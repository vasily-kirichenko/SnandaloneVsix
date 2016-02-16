using System.ComponentModel.Composition;
using System.Windows.Media;
using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Utilities;

namespace StandaloneVsix
{
    #region Format definition
    /// <summary>
    /// Defines an editor format for the StandaloneVsix type that has a purple background
    /// and is underlined.
    /// </summary>
    [Export(typeof(EditorFormatDefinition))]
    [ClassificationType(ClassificationTypeNames = "StandaloneVsix")]
    [Name("StandaloneVsix")]
    [UserVisible(true)] //this should be visible to the end user
    [Order(Before = Priority.Default)] //set the priority to be after the default classifiers
    internal sealed class StandaloneVsixFormat : ClassificationFormatDefinition
    {
        /// <summary>
        /// Defines the visual format for the "StandaloneVsix" classification type
        /// </summary>
        public StandaloneVsixFormat()
        {
            this.DisplayName = "StandaloneVsix"; //human readable version of the name
            this.BackgroundColor = Colors.BlueViolet;
            this.TextDecorations = System.Windows.TextDecorations.Underline;
        }
    }
    #endregion //Format definition
}
