// Guids.cs
// MUST match guids.h
using System;

namespace Pundit.Vsix
{
    static class GuidList
    {
        public const string guidPundit_VsixPkgString = "bc15a5f2-9b23-42ff-820a-d67674e3a80f";
        public const string guidPundit_VsixCmdSetString = "0bb28b3c-754f-40d1-8c82-36295a0260ca";
        public const string guidToolWindowPersistanceString = "9ae988a3-0185-481d-9c3a-f9e8fa32b528";

        public static readonly Guid guidPundit_VsixCmdSet = new Guid(guidPundit_VsixCmdSetString);
    };
}