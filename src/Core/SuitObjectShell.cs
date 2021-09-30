#nullable enable
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace PlasticMetal.MobileSuit.Core
{
    /// <summary>
    /// A collection contains ordered suit shell members.
    /// </summary>
    public interface ISuitShellCollection
    {
        /// <summary>
        /// Ordered members of this
        /// </summary>
        IEnumerable<SuitShell> Members();
    }
    /// <summary>
    ///     Represents an object in Mobile Suit.
    /// </summary>
    public class SuitObjectShell : SuitShell, ISuitShellCollection
    {
        /// <summary>
        ///     The BindingFlags stands for IgnoreCase,  Public and Instance members
        /// </summary>
        public const BindingFlags Flags = BindingFlags.IgnoreCase
                                          | BindingFlags.Public
                                          | BindingFlags.Instance;

        private static readonly SortedSet<string> BlockList = new() { "ToString", "Equals", "GetHashCode", "GetType" };
        private readonly InstanceFactory _instanceFactory;
        private readonly List<SuitShell> _subSystems = new();
        /// <summary>
        /// Create a SuitObjectShell from an instance property
        /// </summary>
        /// <param name="property"></param>
        /// <param name="instanceFactory"></param>
        /// <returns></returns>
        public static SuitObjectShell FromInstanceProperty(PropertyInfo property, InstanceFactory instanceFactory)
        {
            var infoTag = property.GetCustomAttribute<SuitInfoAttribute>();
            var sh = new SuitObjectShell(property.PropertyType,
                c => property.GetValue(instanceFactory(c)), infoTag?.Text ?? property.Name, property.Name)
            {
                Type = infoTag is null ? MemberType.FieldWithoutInfo : MemberType.FieldWithInfo
            };
            if (infoTag is not null) return sh;
            sh.Information = SuitBuildTools.GetMemberInfo(sh);
            return sh;
        }

        /// <summary>
        /// Create a SuitObjectShell from an instance
        /// </summary>
        /// <param name="type"></param>
        /// <param name="instanceFactory"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static SuitObjectShell FromInstance(Type type, InstanceFactory instanceFactory, string name = "")
        {
            var infoTag = type.GetCustomAttribute<SuitInfoAttribute>();
            var sh = new SuitObjectShell(type,
                instanceFactory, infoTag?.Text ?? type.Name, name)
            {
                Type = infoTag is null ? MemberType.FieldWithoutInfo : MemberType.FieldWithInfo
            };
            if (infoTag is not null) return sh;
            sh.Information = SuitBuildTools.GetMemberInfo(sh);
            return sh;
        }
        /// <summary>
        /// Create a SuitObjectShell from an instance field
        /// </summary>
        /// <param name="field"></param>
        /// <param name="instanceFactory"></param>
        /// <returns></returns>
        public static SuitObjectShell FromInstanceField(FieldInfo field, InstanceFactory instanceFactory)
        {
            var infoTag = field.GetCustomAttribute<SuitInfoAttribute>();
            var sh = new SuitObjectShell(field.FieldType,
                s => field.GetValue(instanceFactory(s)), infoTag?.Text ?? field.Name, field.Name)
            {
                Type = infoTag is null ? MemberType.FieldWithoutInfo : MemberType.FieldWithInfo
            };
            if (infoTag is not null) return sh;
            sh.Information = SuitBuildTools.GetMemberInfo(sh);
            return sh;
        }
        /// <summary>
        /// Create a SuitObjectShell from a Type.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static SuitObjectShell FromType(Type type, string name = "")
        {
            object? InstanceFactory(SuitContext s)
            {
                return SuitBuildTools.CreateInstance(type, s) ?? new object();
            }
            var infoTag = type.GetCustomAttribute<SuitInfoAttribute>();
            return new(type, InstanceFactory, infoTag?.Text ?? type.Name, name)
            {
                Type = MemberType.FieldWithInfo
            };
        }

        /// <summary>
        ///     Initialize a SuitObject with an instance.
        /// </summary>
        /// <param name="type">The instance that this SuitObject represents.</param>
        /// <param name="factory"></param>
        /// <param name="info"></param>
        /// <param name="name"></param>
        public SuitObjectShell(Type type, InstanceFactory factory, string info, string name) : base(type, factory, name)
        {
            _instanceFactory = factory;
            if (type is null) return;
            foreach (var member in type.GetMembers(Flags))
            {
                if (member is MethodInfo method)
                {
                    AddMethod(method);
                }
                else if (member is FieldInfo field)
                {
                    AddField(field);
                }
                else if (member is PropertyInfo property)
                {
                    AddProperty(property);
                }
            }

            _subSystems.Sort((l, r) =>
            {
                var abs = string.Compare(l.AbsoluteName, r.AbsoluteName, true, CultureInfo.CurrentUICulture);
                if (abs == 0) return l.MemberCount - r.MemberCount;
                return abs;
            });
            Information = info;
        }

        private void AddMethod(MethodBase method)
        {
            if (method.IsStatic || !method.IsPublic ||
                method.IsConstructor || method.IsSpecialName ||
                method.GetCustomAttribute<SuitIgnoreAttribute>() is not null ||
                BlockList.Contains(method.Name)) return;
            _subSystems.Add(SuitMethodShell.FromInstance(method, _instanceFactory));

        }
        private void AddProperty(PropertyInfo property)
        {
            if (property.GetCustomAttribute<SuitIncludedAttribute>() is null ||
               (property.GetMethod?.IsStatic ?? true) || !(property.GetMethod?.IsPublic ?? false)) return;
            _subSystems.Add(FromInstanceProperty(property, _instanceFactory));

        }
        private void AddField(FieldInfo field)
        {
            if (field.GetCustomAttribute<SuitIncludedAttribute>() is null ||
                field.IsStatic || !field.IsPublic) return;
            _subSystems.Add(FromInstanceField(field, _instanceFactory));

        }
        /// <inheritdoc/>
        public override async Task Execute(SuitContext context)
        {
            var origin = context.Request;
            if(!string.IsNullOrEmpty(AbsoluteName))
                context.Request = origin[1..];
            foreach (var sys in _subSystems.Where(sys => sys.MayExecute(context.Request.ToImmutableArray())))
            {

                await sys.Execute(context);
                if (context.Status != RequestStatus.NotHandled) return;
            }

            context.Request = origin;
        }
        /// <inheritdoc/>
        public override bool MayExecute(IReadOnlyList<string> request)
        {
            if (string.IsNullOrEmpty(AbsoluteName))
            {
                return request.Count > 0 &&
                       _subSystems.Any(sys => sys.MayExecute(request.ToImmutableArray()));
            }
            return request.Count > 1 && FriendlyNames.Contains(request[0],StringComparer.OrdinalIgnoreCase) &&
                   _subSystems.Any(sys => sys.MayExecute(request.Skip(1).ToImmutableArray()));
        }
        /// <summary>
        /// Ordered members of this
        /// </summary>
        public IEnumerable<SuitShell> Members() => _subSystems.AsReadOnly();


        /// <inheritdoc/>
        public override int MemberCount => _subSystems.Count;
    }
}