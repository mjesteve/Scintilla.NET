using System;
using System.Runtime.InteropServices;
using Windows.Win32;

namespace ScintillaNET;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

public static partial class NativeMethods
{
    #region Constants

    public const uint WM_REFLECT = PInvoke.WM_USER + 0x1C00;

    // [IEST] Mensaje que el Shell de Windows envía a una ventana con WS_EX_ACCEPTFILES
    // cuando el usuario suelta ficheros sobre ella. Es independiente del mecanismo OLE
    // IDropTarget, por lo que puede coexistir con el drag-and-drop nativo de texto de
    // Scintilla sin conflicto entre ambos sistemas.
    public const uint WM_DROPFILES = 0x0233u;

    #endregion Constants

    #region Functions

    [DllImport("User32.dll", SetLastError = true)]
    public static extern IntPtr MB_GetString(uint hInst);

    // [IEST] Activa o desactiva en una ventana la recepción del mensaje WM_DROPFILES.
    // Internamente añade o quita el estilo extendido WS_EX_ACCEPTFILES del HWND.
    // Debe llamarse con fAccept=true para que el Shell envíe WM_DROPFILES al soltar
    // ficheros sobre el control (siempre que el IDropTarget activo rechace CF_HDROP).
    [DllImport("shell32.dll")]
    public static extern void DragAcceptFiles(IntPtr hwnd, bool fAccept);

    // [IEST] Lee el nombre de un fichero del handle HDROP que llega en WM_DROPFILES.
    // Pasando iFile = 0xFFFFFFFF devuelve el número total de ficheros en el drop.
    // Hay que llamar a DragFinish cuando se terminen de leer todos los nombres.
    [DllImport("shell32.dll", CharSet = CharSet.Unicode)]
    public static extern uint DragQueryFile(IntPtr hDrop, uint iFile,
                                            System.Text.StringBuilder lpszFile, uint cch);

    // [IEST] Libera la memoria del handle HDROP recibido en WM_DROPFILES.
    // Debe llamarse siempre al terminar de procesar el mensaje para evitar fugas.
    [DllImport("shell32.dll")]
    public static extern void DragFinish(IntPtr hDrop);

    public static string GetMessageBoxString(uint msgId) =>
        Marshal.PtrToStringUni(MB_GetString(msgId));

    #endregion Functions

    #region Structures

    // http://www.openrce.org/articles/full_view/23
    // It's worth noting that this structure (and the 64-bit version below) represents the ILoader
    // class virtual function table (vtable), NOT the ILoader interface defined in ILexer.h.
    // In this case they are identical because the ILoader class contains only functions.
    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct ILoaderVTable32
    {
        public ReleaseDelegate Release;
        public AddDataDelegate AddData;
        public ConvertToDocumentDelegate ConvertToDocument;

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate int ReleaseDelegate(IntPtr self);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate int AddDataDelegate(IntPtr self, byte* data, int length);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate IntPtr ConvertToDocumentDelegate(IntPtr self);
    }

    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct ILoaderVTable64
    {
        public ReleaseDelegate Release;
        public AddDataDelegate AddData;
        public ConvertToDocumentDelegate ConvertToDocument;

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate int ReleaseDelegate(IntPtr self);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate int AddDataDelegate(IntPtr self, byte* data, int length);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate IntPtr ConvertToDocumentDelegate(IntPtr self);
    }

    #endregion Structures
}
