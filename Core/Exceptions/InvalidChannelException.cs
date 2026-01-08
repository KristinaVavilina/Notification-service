using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Exceptions;

public class InvalidChannelException : Exception
{
    public InvalidChannelException(string? message) : base(message)
    {
    }
}
