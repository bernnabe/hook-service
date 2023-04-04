using System;
using System.Collections.Generic;
using System.Text;
using ServiceHooks.Common;

namespace ServiceHooks.Service
{
    public interface IWebHooksService
    {
        OperationResult Do(string code);
    }
}
