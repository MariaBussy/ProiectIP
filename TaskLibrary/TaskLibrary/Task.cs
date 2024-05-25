using System;

namespace TaskLibrary
{
    public class Task
    {
        private String _numeTask;
        private DateTime _dataAsignarii;
        private double _oreLogate;
        private String _descriereTask;
        private String _numeAssigner;

        /// <summary>
        /// Constructor to initialize a new Task with a name and assigner.
        /// </summary>
        /// <param name="nume">The name of the task, which must be at least 5 characters long.</param>
        /// <param name="numePersoana">The name of the assigner, which must be at least 5 characters long.</param>
        /// <exception cref="Exception">Thrown when the task name or assigner name is invalid.</exception>
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

        /// <summary>
        /// Gets or sets the name of the task.
        /// </summary>
        /// <exception cref="Exception">Thrown when the task name is invalid.</exception>
        public string NumeTask
        {
            get { return _numeTask; }
            set
            {
                if (value == null || value == "" || value.Length < 5)
                {
                    throw new Exception("Invalid task name. -> Task name has at least 5 letters.");
                }
                else
                {
                    _numeTask = value;
                }
            }
        }

        /// <summary>
        /// Gets the assignment date of the task in "dd-MM-yyyy" format.
        /// </summary>
        public String DataAsignarii
        {
            get { return _dataAsignarii.ToString("dd-MM-yyyy"); }
        }

        /// <summary>
        /// Gets or sets the logged hours for the task.
        /// </summary>
        /// <exception cref="Exception">Thrown when the value for logged hours is less than or equal to 0.</exception>
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

        /// <summary>
        /// Gets or sets the description of the task.
        /// </summary>
        public String DescriereTask
        {
            get { return _descriereTask; }
            set { _descriereTask = value; }
        }

        /// <summary>
        /// Gets or sets the name of the assigner.
        /// </summary>
        /// <exception cref="Exception">Thrown when the assigner name is invalid.</exception>
        public String NumeAssigner
        {
            get { return _numeAssigner; }
            set
            {
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
