namespace NFugue.Theory
{
    public class Scale
    {
        public Scale(string intervalsString)
            : this(new Intervals(intervalsString))
        { }

        public Scale(string intervalsString, string name)
            : this(new Intervals(intervalsString), name)
        { }

        public Scale(Intervals pattern)
        {
            Intervals = pattern;
        }

        public Scale(Intervals intervals, string name)
        {
            Name = name;
            Intervals = intervals;
        }

        public static readonly int MajorIndicator = 1;
        public static readonly int MinorIndicator = -1;

        public static readonly Scale Major = new Scale(new Intervals("1 2 3 4 5 6 7")) { MajorOrMinorIndicator = MajorIndicator };
        public static readonly Scale Minor = new Scale(new Intervals("1 2 b3 4 5 b6 b7")) { MajorOrMinorIndicator = MinorIndicator };
        public static readonly Scale CircleOfFiths = new Scale(new Intervals("1 2 b3 4 5 6 7b"));

        public string Name { get; set; }

        //TODO: enum?
        public int MajorOrMinorIndicator { get; set; }
        public Intervals Intervals { get; }
        public int Disposition => MajorOrMinorIndicator == MajorIndicator ? 1 : -1;

        #region Equality members
        protected bool Equals(Scale other)
        {
            return Equals(Intervals, other.Intervals);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Scale)obj);
        }

        public override int GetHashCode()
        {
            return Intervals?.GetHashCode() ?? 0;
        }
        #endregion
    }
}