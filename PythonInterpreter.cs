using System;
using System.Diagnostics;

namespace PythonBinding
{
    public class PythonInterpreter
    {
        private static PyModule mainModule = null;

        /// The main Python module. Singleton property, initializes Python environment on first access.
        public static PyModule MainModule
        {
            get
            {
                if (mainModule == null)
                {
                    CPython.Py_SetProgramName(Process.GetCurrentProcess().MainModule.FileName);
                    CPython.Py_Initialize();
                    var exitCode = CPython.PyRun_SimpleString(CPython.StartupScript);
                    if (exitCode == -1) throw new Exception("PyRun_SimpleString did not execute successfully.");
                    unsafe
                    {
                        var sysModDict = CPython.PyImport_GetModuleDict();
                        var rawMainModule = CPython.PyMapping_GetItemString(sysModDict, "__main__");
                        mainModule = new PyModule(rawMainModule);
                    }
                }
                return mainModule;
            }
        }
    }
}
