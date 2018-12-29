using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThreadTest
{
    [Flags]
    public enum Animal
    {
        Dog = 0x01,
        Cat = 0x02,
        Bird = 0x100,
    }

    public class AnimalTest
    {
        public string GetAnimalString()
        {
            return Enum.GetName(typeof(Animal), Animal.Dog);
        }

        public void PrintAnimals()
        {
            foreach(Animal animal in Enum.GetValues(typeof(Animal)))
            {
                Debug.WriteLine(animal);
            }
        }

        public Animal GetAnimal()
        {
            // ToObject: int -> enum
            return (Animal)Enum.ToObject(typeof(Animal), 1);
        }

        public Animal ConvertStirngToAnimal()
        {
            return (Animal)Enum.Parse(typeof(Animal), "Dog");
        }

        // 拡張メソッド
    }
}
