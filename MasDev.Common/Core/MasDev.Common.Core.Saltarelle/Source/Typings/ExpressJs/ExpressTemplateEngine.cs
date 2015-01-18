using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace NodeExpress
{
    public delegate void ExpressTemplateEngine(string path, object locals, Action<string> callback);
}
