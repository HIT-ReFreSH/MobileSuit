using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static PlasticMetal.MobileSuit.SuitBuildTools;

namespace PlasticMetal.MobileSuit.Core.Middleware
{
    /// <summary>
    /// Middleware which provides user input
    /// </summary>
    public class UserInputMiddleware : ISuitMiddleware
    {
        /// <inheritdoc/>
        public async Task InvokeAsync(SuitContext context, SuitRequestDelegate next)
        {
            if (context.CancellationToken.IsCancellationRequested)
            {
                context.Status = RequestStatus.Interrupt;
                await next(context);
            }
            if (context.Status == RequestStatus.NoRequest)
            {
                var io = context.ServiceProvider.GetRequiredService<IIOHub>();
                var originInput = await io.ReadLineAsync();
                if (originInput is null)
                {
                    context.Status = RequestStatus.OnExit;
                    return;
                }
                if (originInput.StartsWith("#"))
                {
                    context.Status = RequestStatus.Ok;
                    return;
                }

                var spl = SplitCommandLine(originInput);
                var status = 0;
                var request = new List<string>();
                foreach (var unit in spl)
                {
                    switch (status)
                    {
                        case 0:
                            if (!unit.StartsWith("#") && unit.Length == 2)
                            {
                                switch (unit[1])
                                {
                                    case '>':
                                        status = 1;
                                        break;
                                    case '<':
                                        status = -1;
                                        break;
                                    case '!':
                                        context.Properties.Add(SuitCommandTarget, SuitCommandTargetClient);
                                        break;
                                    case '@':
                                        context.Properties.Add(SuitCommandTarget, SuitCommandTargetServer);
                                        break;
                                    case '&':
                                        context.Properties.Add(SuitAsTask, string.Empty);
                                        break;
                                    default:
                                        context.Status = RequestStatus.CommandParsingFailure;
                                        status = 2;
                                        break;
                                }
                            }
                            else
                            {
                                request.Add(unit);
                            }

                            break;
                        case 1:
                            io.Output = new StreamWriter(File.OpenWrite(unit));
                            status = 0;
                            break;
                        case -1:
                            io.Input = new StreamReader(File.OpenRead(unit));
                            status = 0;
                            break;
                        case 2:
                            await next(context);
                            return;
                    }

                }
                if (request.Count == 0)
                {
                    context.Status = RequestStatus.Ok;
                    return;
                }

                context.Status = RequestStatus.NotHandled;
                context.Request = request.ToArray();
            }
            await next(context);
        }
        /// <summary>
        ///     Split a commandline string to args[] array.
        /// </summary>
        /// <param name="commandLine">commandline string</param>
        /// <returns>args[] array</returns>
        private static unsafe IList<string> SplitCommandLine(string commandLine)
        {
            if (string.IsNullOrEmpty(commandLine)) return Array.Empty<string>();
            var l = new List<string>();
            /*
             * Node:
             * S0: Default status
             * S1: InQuotes
             * S2: CodeNextChar
             * S3: Comment
             * S4: AfterSpace
             *
             * Edge
             * S0--',"->S1
             * S1--',"->S0
             * S0,S1--\->S2
             * S2-->S0,S1
             * S0--#->S3
             * S0-- ->S4
             * S4-->S0
             */
            var status = 4;
            var quote = '\'';
            var lastStatus = 0;
            var i = 0;
            var buf = stackalloc char[256];
            foreach (var c in commandLine)
            {
                switch (status)
                {
                    case 0:
                        switch (c)
                        {
                            case '\'':
                            case '"':
                                quote = c;
                                status = 1;
                                continue;
                            case '\\':
                                buf[i++] = c;
                                lastStatus = 0;
                                status = 2;
                                continue;
                            case '#':
                                status = 3;
                                break;
                            case ' ':
                                status = 4;
                                if (i > 0)
                                    l.Add(Regex.Unescape(new string(buf, 0, i)));
                                i = 0;
                                continue;
                            default:
                                buf[i++] = c;
                                continue;
                        }

                        break;
                    case 1:
                        if (c == quote)
                        {
                            status = 0;
                            l.Add(Regex.Unescape(new string(buf, 0, i)));
                            i = 0;
                            continue;
                        }
                        else if (c == '\\')
                        {
                            buf[i++] = c;
                            lastStatus = 1;
                            status = 2;
                            continue;
                        }
                        else
                        {
                            buf[i++] = c;
                            continue;
                        }
                    case 2:
                        buf[i++] = c;
                        status = lastStatus;
                        continue;
                    case 4:
                        if (c is '#')
                        {
                            break;
                        }
                        else switch (c)
                            {
                                case '\\':
                                    buf[i++] = c;
                                    lastStatus = 0;
                                    status = 2;
                                    break;
                                case '@' or '!' or '&' or '<' or '>':
                                    l.Add($"#{c}");
                                    break;
                                default:
                                    buf[i++] = c;
                                    status = 0;
                                    break;
                            }

                        continue;

                }
                break;
            }

            if (i > 0)
                l.Add(Regex.Unescape(new string(buf, 0, i)));
            return l;
        }
    }
}
