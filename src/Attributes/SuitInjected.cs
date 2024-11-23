using System;

namespace HitRefresh.MobileSuit;

/// <summary>
///     Indicate that Mobile Suit should Inject to this Object.
/// </summary>
[AttributeUsage(AttributeTargets.Parameter)]
public sealed class SuitInjected : Attribute { }