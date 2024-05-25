using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskLibrary
{
    public class Task
    {
        private String _numeTask;
        private DateTime _dataAsignarii;
        private double _oreLogate;
        private String _descriereTask;
        private String _numeAssigner;

        public Task(in String nume, in String numePersoana)
        {
            if (nume == null || nume == "" || nume.Length < 5)
            {
                throw new Exception("Invalid task name. -> Task name has at least 5 letters.");
            }
            else
            {
                _numeTask = nume;
            }

            if (numePersoana == null || numePersoana == "" || numePersoana.Length < 5)
            {
                throw new Exception("Invalid assigner name. -> Assigner name has at least 5 letters.");
            }
            else
            {
                _numeAssigner = numePersoana;
            }

            _dataAsignarii = DateTime.Now;

            _descriereTask = "";
            _oreLogate = 0.0;
        }

        public string NumeTask
        {
            get { return _numeTask; }
            set {
                if (value == null || value == "" || value.Length<5)
                {
                    throw new Exception("Invalid task name. -> Task name has at least 5 letters.");
                }
                else
                {
                    _numeTask = value;
                }
            }
        }

        public String DataAsignarii
        {
            get { return _dataAsignarii.ToString("dd-MM-yyyy"); }
        }

        public double OreLogate
        {
            get { return _oreLogate; }
            set
            {
                if (value <= 0.0)
                {
                    throw new Exception("Invalid value for logged hours.-> value>0");
                }
                else
                {
                    _oreLogate = value;
                }
            }
        }

        public String DescriereTask
        {
            get { return _descriereTask; }
            set { _descriereTask = value; }
        }

        public String NumeAssigner
        {
            get { return _numeAssigner; }
            set {
                if (value == null || value == "" || value.Length < 5)
                {
                    throw new Exception("Invalid assigner name. -> Assigner name has at least 5 letters.");
                }
                else
                {
                    _numeAssigner = value;
                }
            }
        }
    }
}
