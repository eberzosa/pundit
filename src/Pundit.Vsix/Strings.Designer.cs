﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.239
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Pundit.Vsix {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class Strings {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Strings() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("Pundit.Vsix.Strings", typeof(Strings).Assembly);
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
        ///   Looks up a localized string similar to Visual Studio has no idea why the window cannot be created.
        /// </summary>
        internal static string CantCreateToolWindow {
            get {
                return ResourceManager.GetString("CantCreateToolWindow", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Pundit {0}.
        /// </summary>
        internal static string ConsoleIntro {
            get {
                return ResourceManager.GetString("ConsoleIntro", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Pundit Console.
        /// </summary>
        internal static string PunditConsoleWindowTitle {
            get {
                return ResourceManager.GetString("PunditConsoleWindowTitle", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to This solution has no valid Pundit manifest file expected at location {0}.
        /// </summary>
        internal static string SolutionHasNoManifest {
            get {
                return ResourceManager.GetString("SolutionHasNoManifest", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Could not access solution properties. Is any solution open at at all at the moment?.
        /// </summary>
        internal static string SolutionNotAccessible {
            get {
                return ResourceManager.GetString("SolutionNotAccessible", resourceCulture);
            }
        }
    }
}
