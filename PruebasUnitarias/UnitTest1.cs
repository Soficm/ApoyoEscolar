using Apoyo.Server.Controllers;
using Microsoft.EntityFrameworkCore.Query.Internal;
using NUnit.Framework;
using System;

namespace PruebasUnitarias
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Test1()
        {
            CuentaController x = new CuentaController();
            try {
                x.Sumar(2, 3);
                Assert.Pass();
            }
            catch(Exception e)
            {
                Assert.Fail(e.Message);
            }
            
        }
        [Test]
        public void Test2()
        {
            CuentaController x = new CuentaController();
            try
            {
                x.Sumar(0, 3);
                Assert.Pass();
            }
            catch (Exception e)
            {
                Assert.Fail(e.Message);
            }

        }
    }
}