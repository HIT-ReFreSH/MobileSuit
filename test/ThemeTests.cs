using System.Drawing;
using Xunit;
using HitRefresh.MobileSuit.Themes;
using HitRefresh.MobileSuit.Core;

namespace HitRefresh.MobileSuit.Tests
{
    public class ThemeTests
    {
        [Fact]
        public void NordTheme_HasCorrectColors()
        {
            // Arrange
            var theme = new NordTheme();

            // Assert - 验证 Nord 官方颜色
            Assert.Equal(Color.FromArgb(216, 222, 233), theme.DefaultColor);     // Nord6
            Assert.Equal(Color.FromArgb(136, 192, 208), theme.PromptColor);      // Nord8
            Assert.Equal(Color.FromArgb(191, 97, 106), theme.ErrorColor);        // Nord11
            Assert.Equal(Color.FromArgb(163, 190, 140), theme.OkColor);          // Nord14
            Assert.Equal(Color.FromArgb(94, 129, 172), theme.TitleColor);        // Nord10
            Assert.Equal(Color.FromArgb(129, 161, 193), theme.InformationColor); // Nord9
            Assert.Equal(Color.FromArgb(46, 52, 64), theme.SystemColor);         // Nord0
            Assert.Equal(Color.FromArgb(235, 203, 139), theme.WarningColor);     // Nord13
            Assert.Equal(Color.FromArgb(46, 52, 64), theme.BackgroundColor);     // Nord0
        }

        [Fact]
        public void DraculaTheme_HasCorrectColors()
        {
            var theme = new DraculaTheme();

            // 验证 Dracula 官方颜色
            Assert.Equal(Color.FromArgb(248, 248, 242), theme.DefaultColor);
            Assert.Equal(Color.FromArgb(189, 147, 249), theme.PromptColor);
            Assert.Equal(Color.FromArgb(255, 85, 85), theme.ErrorColor);
            Assert.Equal(Color.FromArgb(80, 250, 123), theme.OkColor);
            Assert.Equal(Color.FromArgb(255, 121, 198), theme.TitleColor);
            Assert.Equal(Color.FromArgb(139, 233, 253), theme.InformationColor);
            Assert.Equal(Color.FromArgb(68, 71, 90), theme.SystemColor);
            Assert.Equal(Color.FromArgb(241, 250, 140), theme.WarningColor);
            Assert.Equal(Color.FromArgb(40, 42, 54), theme.BackgroundColor);
        }

        [Fact]
        public void SolarizedLightTheme_HasCorrectColors()
        {
            var theme = new SolarizedLightTheme();

            // 验证 Solarized Light 官方颜色
            Assert.Equal(Color.FromArgb(101, 123, 131), theme.DefaultColor);     // Base00
            Assert.Equal(Color.FromArgb(38, 139, 210), theme.PromptColor);       // Blue
            Assert.Equal(Color.FromArgb(220, 50, 47), theme.ErrorColor);         // Red
            Assert.Equal(Color.FromArgb(133, 153, 0), theme.OkColor);            // Green
            Assert.Equal(Color.FromArgb(181, 137, 0), theme.TitleColor);         // Yellow
            Assert.Equal(Color.FromArgb(42, 161, 152), theme.InformationColor);  // Cyan
            Assert.Equal(Color.FromArgb(238, 232, 213), theme.SystemColor);      // Base2
            Assert.Equal(Color.FromArgb(203, 75, 22), theme.WarningColor);       // Orange
            Assert.Equal(Color.FromArgb(253, 246, 227), theme.BackgroundColor);  // Base3
        }

        [Fact]
        public void SolarizedDarkTheme_HasCorrectColors()
        {
            var theme = new SolarizedDarkTheme();

            // 验证 Solarized Dark 官方颜色
            Assert.Equal(Color.FromArgb(131, 148, 150), theme.DefaultColor);     // Base0
            Assert.Equal(Color.FromArgb(38, 139, 210), theme.PromptColor);       // Blue
            Assert.Equal(Color.FromArgb(220, 50, 47), theme.ErrorColor);         // Red
            Assert.Equal(Color.FromArgb(133, 153, 0), theme.OkColor);            // Green
            Assert.Equal(Color.FromArgb(181, 137, 0), theme.TitleColor);         // Yellow
            Assert.Equal(Color.FromArgb(42, 161, 152), theme.InformationColor);  // Cyan
            Assert.Equal(Color.FromArgb(0, 43, 54), theme.SystemColor);          // Base03
            Assert.Equal(Color.FromArgb(203, 75, 22), theme.WarningColor);       // Orange
            Assert.Equal(Color.FromArgb(0, 43, 54), theme.BackgroundColor);      // Base03
        }

        [Fact]
        public void MonokaiTheme_HasCorrectColors()
        {
            var theme = new MonokaiTheme();

            // 验证 Monokai 颜色
            Assert.Equal(Color.FromArgb(248, 248, 242), theme.DefaultColor);
            Assert.Equal(Color.FromArgb(249, 38, 114), theme.PromptColor);
            Assert.Equal(Color.FromArgb(255, 0, 0), theme.ErrorColor);
            Assert.Equal(Color.FromArgb(166, 226, 46), theme.OkColor);
            Assert.Equal(Color.FromArgb(102, 217, 239), theme.TitleColor);
            Assert.Equal(Color.FromArgb(174, 129, 255), theme.InformationColor);
            Assert.Equal(Color.FromArgb(39, 40, 34), theme.SystemColor);
            Assert.Equal(Color.FromArgb(230, 219, 116), theme.WarningColor);
            Assert.Equal(Color.FromArgb(39, 40, 34), theme.BackgroundColor);
        }

        [Fact]
        public void Themes_ImplementEqualityCorrectly()
        {
            // Arrange
            var theme1 = new NordTheme();
            var theme2 = new NordTheme();
            var dracula = new DraculaTheme();

            // Act & Assert
            Assert.True(theme1.Equals(theme2));
            Assert.False(theme1.Equals(dracula));
            Assert.False(theme1.Equals(null));
        }
    }
}