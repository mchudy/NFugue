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

        public static readonly Scale Major = new Scale(new Intervals("1 2 3 4 5 6 7")) { Type = ScaleType.Major };
        public static readonly Scale Minor = new Scale(new Intervals("1 2 b3 4 5 b6 b7")) { Type = ScaleType.Minor };
        public static readonly Scale CircleOfFiths = new Scale(new Intervals("1 2 b3 4 5 6 7b"));

        public string Name { get; set; }

        public ScaleType Type { get; set; } = ScaleType.Major;
        public Intervals Intervals { get; }
        public int Disposition => Type == ScaleType.Major ? 1 : -1;

        public override string ToString()
        {
            if (Type == ScaleType.Major)
            {
                return "maj";
            }
            if (Type == ScaleType.Minor)
            {
                return "min";
            }
            return Name;
        }

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

    public enum ScaleType
    {
        Major = 1,
        Minor = -1
    }
}