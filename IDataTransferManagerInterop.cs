namespace TranscribeMe {

    [ComImport]
    [Guid("3A3DCD6C-3EAB-43DC-BCDE-45671CE800C8")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    interface IDataTransferManagerInterop {
        IntPtr GetForWindow(IntPtr appWindow, in Guid riid);
        void ShowShareUIForWindow(IntPtr appWindow);
    }
}