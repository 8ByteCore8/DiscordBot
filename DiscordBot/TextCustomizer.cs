using System;
using System.Collections.Generic;
using System.Text;

namespace DiscordBot
{
    public static class TextCustomizer
    {
        /// <summary>
        /// Преобразует шрифт в курсивный.
        /// </summary>
        /// <param name="text">Текст для форматирования.</param>
        /// <returns>Отформатированый текст.</returns>
        public static string Italic(string text) => $"*{text}*";

        /// <summary>
        /// Преобразует шрифт в жирный.
        /// </summary>
        /// <param name="text">Текст для форматирования.</param>
        /// <returns>Отформатированый текст.</returns>
        public static string Bold(string text) => $"**{text}**";

        /// <summary>
        /// Преобразует шрифт в жирный курсив.
        /// </summary>
        /// <param name="text">Текст для форматирования.</param>
        /// <returns>Отформатированый текст.</returns>
        public static string BoldItalic(string text) => $"***{text}***";

        /// <summary>
        /// Преобразует шрифт в подчеркнутый.
        /// </summary>
        /// <param name="text">Текст для форматирования.</param>
        /// <returns>Отформатированый текст.</returns>
        public static string Underlined(string text) => $"__{text}__";

        /// <summary>
        /// Преобразует шрифт в подчеркнутый курсив.
        /// </summary>
        /// <param name="text">Текст для форматирования.</param>
        /// <returns>Отформатированый текст.</returns>
        public static string UnderlinedItalic(string text) => $"__*{text}*__";

        /// <summary>
        /// Преобразует шрифт в подчеркнутый жирный.
        /// </summary>
        /// <param name="text">Текст для форматирования.</param>
        /// <returns>Отформатированый текст.</returns>
        public static string UnderlinedBold(string text) => $"__**{text}**__";

        /// <summary>
        /// Преобразует шрифт в подчеркнутый жирный курсив.
        /// </summary>
        /// <param name="text">Текст для форматирования.</param>
        /// <returns>Отформатированый текст.</returns>
        public static string UnderlinedBoldItalic(string text) => $"__***{text}***__";

        /// <summary>
        /// Преобразует шрифт в перечеркнутый.
        /// </summary>
        /// <param name="text">Текст для форматирования.</param>
        /// <returns>Отформатированый текст.</returns>
        public static string Strikethrough(string text) => $"~~{text}~~";

        /// <summary>
        /// Помещяет текст в однострочный блок кода.
        /// </summary>
        /// <param name="text">Текст для форматирования.</param>
        /// <returns>Отформатированый текст.</returns>
        public static string OneLineCodeBlock(string text) => $"`{text}`";

        /// <summary>
        /// Помещяет текст в многострочный блок кода.
        /// </summary>
        /// <param name="text">Текст для форматирования.</param>
        /// <returns>Отформатированый текст.</returns>
        public static string OneLineCodeBlock(string text,string lang = "") => $"```{lang}\n{text}\n```";
    }
}
