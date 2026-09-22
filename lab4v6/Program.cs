using System;

class LessonDuration
{
    private int _hours;
    private int _minutes;
    private int _seconds;

    public int Hours
    {
        get {return _hours;}
        set
        {
            if (value >= 0 && value <= 23)
            {
                _hours = value;
            }
        }
    }
    public int Minutes
    {
        get {return _minutes;}
        set
        {
            if (value >= 0 && value <= 59)
            {
                _minutes = value;
            }
        }
    }

    public int Seconds
    {
        get {return _seconds;}
        set
        {
            if (value >= 0 && value <= 59)
            {
                _seconds = value;
            }
        }
    }

    public LessonDuration(int hours, int minutes, int seconds)
    {
        _hours = hours;
        _minutes = minutes;
        _seconds = seconds;
    }
    public static LessonDuration Zero
    {
        get {return new LessonDuration(0, 0, 0);}
    }

    public static LessonDuration operator +(LessonDuration a, LessonDuration b)
    {
        int seconds = a.Seconds + b.Seconds;
        int minutes = a.Minutes + b.Minutes;
        int hours = a.Hours + b.Hours;

        if (seconds >= 60)
        {
            seconds -= 60;
            minutes++;
        }

        if (minutes >= 60)
        {
            minutes -= 60;
            hours++;
        }

        if (hours >= 24)
        {
            hours -= 24;
        }

        return new LessonDuration(hours, minutes, seconds);
    }

    public static LessonDuration operator -(LessonDuration a, LessonDuration b)
    {
        int seconds = a.Seconds - b.Seconds;
        int minutes = a.Minutes - b.Minutes;
        int hours = a.Hours - b.Hours;

        if (seconds < 0)
        {
            seconds += 60;
            minutes--;
        }

        if (minutes < 0)
        {
            minutes += 60;
            hours--;
        }

        if (hours < 0)
        {
            hours = 0;
            minutes = 0;
            seconds = 0;
        }

        return new LessonDuration(hours, minutes, seconds);
    }

    public override string ToString()
    {
        return $"{Hours}:{Minutes}:{Seconds}";
    }

    public override bool Equals(Object? obj)
    {
        if ((obj == null) || !this.GetType().Equals(obj.GetType()))
        {
            return false;
        }

        else
        {
            LessonDuration other = (LessonDuration)obj;

            return (Hours == other.Hours) && (Minutes == other.Minutes) && (Seconds == other.Seconds);
        }
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Hours, Minutes, Seconds);
    }

    public static bool operator ==(LessonDuration a, LessonDuration b)
    {
        return a.Equals(b);
    }

    public static bool operator !=(LessonDuration a, LessonDuration b)
    {
        return !a.Equals(b);
    }
}

class Program
{
    static void Main()
    {
        LessonDuration time_1 = new LessonDuration(5, 55, 59);
        LessonDuration time_2 = new LessonDuration(20, 35, 15);
        LessonDuration time_3 = new LessonDuration(5, 55, 59);

        Console.WriteLine("Hours: " + time_1.Hours);
        Console.WriteLine("Minutes: " + time_1.Minutes);
        Console.WriteLine("Seconds: " + time_1.Seconds);

        time_1.Minutes = 70;
        Console.WriteLine("Minutes after validation: " + time_1.Minutes);

        LessonDuration zero = LessonDuration.Zero;
        Console.WriteLine("Zero: " + zero);

        LessonDuration sum = time_1 + time_2;
        Console.WriteLine("Sum: " + sum);

        LessonDuration difference = time_1 - time_2;
        Console.WriteLine("difference: " + difference);

        Console.WriteLine("Equals: " + time_1.Equals(time_3));

        Console.WriteLine("==: " + (time_1 == time_3));

        Console.WriteLine("!=: " + (time_1 != time_3));
    }
}