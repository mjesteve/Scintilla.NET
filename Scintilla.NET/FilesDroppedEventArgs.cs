// [IEST] Fichero nuevo — no existe en el repo oficial de Scintilla.NET.
// Contiene los datos del evento FilesDropped que se dispara cuando el usuario arrastra
// ficheros desde el Explorador de Windows sobre un control Scintilla con AcceptFileDrop=true.

using System;

namespace ScintillaNET;

/// <summary>
/// [IEST] Proporciona datos para el evento <see cref="Scintilla.FilesDropped"/>.
/// Contiene las rutas completas de todos los ficheros soltados sobre el control.
/// </summary>
public class FilesDroppedEventArgs : EventArgs
{
    /// <summary>
    /// [IEST] Rutas completas de los ficheros recibidos en el drop, en el orden
    /// en que el Shell los proporcionó a través del handle HDROP.
    /// </summary>
    public string[] FileNames { get; }

    /// <summary>
    /// [IEST] Inicializa una nueva instancia con la lista de ficheros arrastrados.
    /// </summary>
    /// <param name="fileNames">Rutas completas de los ficheros soltados.</param>
    public FilesDroppedEventArgs(string[] fileNames)
    {
        FileNames = fileNames;
    }
}
