using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KSU.CIS300.Snake
{
    /// <summary>
    /// SnakeStatus
    /// </summary>
    public enum SnakeStatus
    {
        Empty,      
        SnakeBody,  
        SnakeHead,  
        SnakeFood,
        Moving,
        Eating,
        Collision,
        InvalidDirection,
        Win

    }
}
