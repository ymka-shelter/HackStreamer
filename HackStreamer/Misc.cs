using System;
using System.Runtime.InteropServices;

namespace Hack
{
    [ComImport, System.Security.SuppressUnmanagedCodeSecurity,
 Guid("6d5140c1-7436-11ce-8034-00aa006009fa"),
 InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IServiceProvider
    {
        [PreserveSig]
        int QueryService(
            [In, MarshalAs(UnmanagedType.LPStruct)] Guid guidService,
            [In, MarshalAs(UnmanagedType.LPStruct)] Guid riid,
            [MarshalAs(UnmanagedType.IUnknown)] out object ppvObject
            );
    }
    [ComImport, Guid("7c23220e-55bb-11d3-8b16-00c04fb6bd3d")]
    public class WMAsfWriter
    {
    }
}
