// /*
//  * Author: Ferdinand Su
//  * Email: ${User.Email}
//  * Date: 11 23, 2024
//  *
//  */

using System;
using System.Drawing;

namespace HitRefresh.MobileSuit;

/// <summary>
///     A basic unit of output print, contains foreground, background and text.
/// </summary>
public struct PrintUnit
{
    /// <summary>
    ///     Text of output.
    /// </summary>
    public string Text { get; set; }

    /// <summary>
    ///     Foreground color of output.
    /// </summary>
    public Color? Foreground { get; set; }

    /// <summary>
    ///     Background color of output.
    /// </summary>
    public Color? Background { get; set; }

    /// <summary>
    ///     Convert from print unit to a tuple
    /// </summary>
    /// <param name="pu">The print unit</param>
    public static implicit operator (string, Color?, Color?)(PrintUnit pu)
    {
        return (pu.Text, pu.Foreground, pu.Background);
    }

    /// <summary>
    ///     Convert ConsoleColor to Color
    /// </summary>
    /// <param name="origin"></param>
    /// <returns></returns>
    public static Color? ConsoleColorCast(ConsoleColor? origin)
    {
        return origin switch
               {
                   ConsoleColor.DarkYellow => Color.Orange,
                   { } v => Color.FromName(Enum.GetName(typeof(ConsoleColor), v) ?? "White"),
                   _ => null
               };
    }

    /// <summary>
    ///     Convert from print unit back to a tuple
    /// </summary>
    /// <param name="tp">The print unit</param>
    public static explicit operator PrintUnit((string, ConsoleColor?, ConsoleColor?) tp)
    {
        return new PrintUnit
               {
                   Text = tp.Item1,
                   Foreground = ConsoleColorCast(tp.Item2),
                   Background = ConsoleColorCast(tp.Item3)
               };
    }

    /// <summary>
    ///     Convert from print unit back to a tuple
    /// </summary>
    /// <param name="tp">The print unit</param>
    public static implicit operator PrintUnit((string, Color?) tp)
    {
        return new PrintUnit
               {
                   Text = tp.Item1,
                   Foreground = tp.Item2,
                   Background = null
               };
    }

    /// <summary>
    ///     Convert from print unit back to a tuple
    /// </summary>
    /// <param name="tp">The print unit</param>
    public static implicit operator PrintUnit((string, Color?, Color?) tp)
    {
        return new PrintUnit
               {
                   Text = tp.Item1,
                   Foreground = tp.Item2,
                   Background = tp.Item3
               };
    }

    /// <summary>
    ///     Convert from print unit back to a tuple
    /// </summary>
    /// <param name="tp">The print unit</param>
    public static explicit operator PrintUnit((string, ConsoleColor?) tp)
    {
        return new PrintUnit
               {
                   Text = tp.Item1,
                   Foreground = ConsoleColorCast(tp.Item2),
                   Background = null
               };
    }
}