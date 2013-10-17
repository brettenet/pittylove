using System.Collections.Generic;
using _02PittyLove.WinRT2.Model;

namespace _02PittyLove.WinRT2.Services
{
    public interface IPittyLoveService
    {
        List<Pitbull> GetDogs();
        Pitbull Save(Pitbull pitbull);
    }
}