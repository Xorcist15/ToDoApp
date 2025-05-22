namespace ToDoApp.Models {
    public class ToDoTask {
        private int _id;
        private string _description;
        private DateTime _dueDate;
        private string _status;

        public ToDoTask (int id, string desc, DateTime date) {
            this._id = id;
            this._description = desc;
            this._dueDate = date;
            this._status = "Not started";
        }

        public int Id {
            get { return _id; }
        }

        public string Description {
            get { return _description; }
            set { _description = value; }
        }

        public DateTime DueDate {
            get { return _dueDate; }
            set { _dueDate = value; }
        }

        public string Status {
            get { return _status; }
            set { _status = value; }
        }

        public void markAsCompleted () {}
        public void markAsPending () {}
        public bool isOverdue() {
            return false;
        }
    }
}
