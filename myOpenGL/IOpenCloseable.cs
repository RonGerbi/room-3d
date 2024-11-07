using System;
using System.Collections.Generic;
using System.Text;

namespace myOpenGL
{
    public interface IOpenCloseable
    {
        void Open();
        void Close();
    }
}
