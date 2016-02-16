using System.ComponentModel.Composition;
using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Utilities;

namespace StandaloneVsix
{
    internal static class StandaloneVsixClassificationDefinition
    {
        /// <summary>
        /// Defines the "StandaloneVsix" classification type.
        /// </summary>
        [Export(typeof(ClassificationTypeDefinition))]
        [Name("StandaloneVsix")]
        internal static ClassificationTypeDefinition StandaloneVsixType = null;
    }
}
