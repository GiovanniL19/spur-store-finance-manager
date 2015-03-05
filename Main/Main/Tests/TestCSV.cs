using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Main.Tests
{
    class TestCSV
    {
        [Test]
        public void Test1()
        {
            String path = "C:\\Users\\Giovanni\\Downloads\\StoreData(1)\\StoreData";

            Folder folder = new Folder();

            folder.getFiles(path);
        }
    }
}
