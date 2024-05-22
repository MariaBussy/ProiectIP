using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskLibrary
{
    public class Task
    {
        private String numeTask;
        private DateTime dataAsignarii;
        private double oreLogate;
        private String descriereTask;
        private String numeAssigner;

        public Task(in String nume, in String numePersoana)
        {
            if (nume == null || nume == "" || nume.Length < 5)
            {
                throw new Exception("Invalid task name. -> Task name has at least 5 letters.");
            }
            else
            {
                numeTask = nume;
            }

            if (numePersoana == null || numePersoana == "" || numePersoana.Length < 5)
            {
                throw new Exception("Invalid assigner name. -> Assigner name has at least 5 letters.");
            }
            else
            {
                numeAssigner = numePersoana;
            }

            dataAsignarii = DateTime.Now;

            descriereTask = "";
            oreLogate = 0.0;
        }

        public string NumeTask
        {
            get { return numeTask; }
            set {
                if (value == null || value == "" || value.Length<5)
                {
                    throw new Exception("Invalid task name. -> Task name has at least 5 letters.");
                }
                else
                {
                    numeTask = value;
                }
            }
        }

        public String DataAsignarii
        {
            get { return dataAsignarii.ToString("dd-MM-yyyy"); }
        }

        public double OreLogate
        {
            get { return oreLogate; }
            set
            {
                if (value <= 0.0)
                {
                    throw new Exception("Invalid value for logged hours.-> value>0");
                }
                else
                {
                    oreLogate = value;
                }
            }
        }

        public String DescriereTask
        {
            get { return descriereTask; }
            set { descriereTask = value; }
        }

        public String NumeAssigner
        {
            get { return numeAssigner; }
            set {
                if (value == null || value == "" || value.Length < 5)
                {
                    throw new Exception("Invalid assigner name. -> Assigner name has at least 5 letters.");
                }
                else
                {
                    numeAssigner = value;
                }
            }
        }
    }
}
