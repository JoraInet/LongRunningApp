﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace LongRunningApp.Api.Properties {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "17.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class Resource {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Resource() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("LongRunningApp.Api.Properties.Resource", typeof(Resource).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Error while cancelling processing text..
        /// </summary>
        internal static string CancelProcessingTextError {
            get {
                return ResourceManager.GetString("CancelProcessingTextError", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Conection Id for processing is empty..
        /// </summary>
        internal static string ConectionIdForProcessingIsEmpty {
            get {
                return ResourceManager.GetString("ConectionIdForProcessingIsEmpty", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Error while staring processing text..
        /// </summary>
        internal static string StartProcessingTextError {
            get {
                return ResourceManager.GetString("StartProcessingTextError", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Text for processing is empty..
        /// </summary>
        internal static string TextForProcessingIsEmpty {
            get {
                return ResourceManager.GetString("TextForProcessingIsEmpty", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to While processing requst an unexpected error occured. Error id:{0}.
        /// </summary>
        internal static string UnexpectedServerErrorWithId {
            get {
                return ResourceManager.GetString("UnexpectedServerErrorWithId", resourceCulture);
            }
        }
    }
}
