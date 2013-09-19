using MartinZottmann.Engine.States;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MartinZottmann.Engine.UnitTests
{
    [TestClass]
    public class States_Inception
    {
        [TestMethod]
        public void Inception_StateMachine()
        {
            // Arrange
            var dictionary = new States_StateMachine.StateDictionary<string, string>();
            var machine = new StateMachine<States_StateMachine.StateDictionary<string, string>, string, string>(dictionary);
            var machine_machine = new StateMachine<
                    IStateMachine<States_StateMachine.StateDictionary<string, string>, string, string>,
                    string,
                    State<string, string>
                >(machine);
            machine_machine.CreateState("0")
                .Add("00", new State<string, string>()
                    .Add("000", "000")
                    .Add("001", "001")
                )
                .Add("01", new State<string, string>()
                    .Add("010", "010")
                    .Add("011", "011")
                );
            machine_machine.CreateState("1")
                .Add("10", new State<string, string>()
                    .Add("100", "100")
                    .Add("101", "101")
                )
                .Add("11", new State<string, string>()
                    .Add("110", "110")
                    .Add("111", "111")
                );

            // Act & Assert
            Assert.AreEqual(dictionary.Count, 0);
            Assert.AreEqual(machine.Count, 0);
            Assert.AreEqual(machine_machine.Count, 2);
            machine_machine.ChangeState("0");
            Assert.AreEqual(dictionary.Count, 0);
            Assert.AreEqual(machine.Count, 2);
            Assert.AreEqual(machine_machine.Count, 2);
            machine.ChangeState("00");
            Assert.AreEqual(dictionary.Count, 2);
            Assert.AreEqual(machine.Count, 2);
            Assert.AreEqual(machine_machine.Count, 2);
            Assert.AreEqual(dictionary["000"], "000");
            Assert.AreEqual(dictionary["001"], "001");
            machine.ChangeState("01");
            Assert.AreEqual(dictionary.Count, 2);
            Assert.AreEqual(machine.Count, 2);
            Assert.AreEqual(machine_machine.Count, 2);
            Assert.AreEqual(dictionary["010"], "010");
            Assert.AreEqual(dictionary["011"], "011");
            machine_machine.ChangeState("1");
            Assert.AreEqual(dictionary.Count, 2); // ?
            Assert.AreEqual(machine.Count, 2);
            Assert.AreEqual(machine_machine.Count, 2);
            machine.ChangeState("10");
            Assert.AreEqual(dictionary.Count, 2);
            Assert.AreEqual(machine.Count, 2);
            Assert.AreEqual(machine_machine.Count, 2);
            Assert.AreEqual(dictionary["100"], "100");
            Assert.AreEqual(dictionary["101"], "101");
            machine.ChangeState("11");
            Assert.AreEqual(dictionary.Count, 2);
            Assert.AreEqual(machine.Count, 2);
            Assert.AreEqual(machine_machine.Count, 2);
            Assert.AreEqual(dictionary["110"], "110");
            Assert.AreEqual(dictionary["111"], "111");
        }
    }
}
