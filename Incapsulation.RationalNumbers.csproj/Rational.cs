using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Incapsulation.RationalNumbers
{
    public class Rational
    {
        public int Numerator { get; set; }
        public int Denominator { get; set; }
        public bool IsNan { get => this.Denominator == 0; }
        
        private void Reduce()
        {
            var m = Math.Abs(this.Numerator);
            var n = Math.Abs(this.Denominator);
            while (m != n)
            {
                if (m > n)
                    m = m - n;
                else
                    n = n - m;
            }
            this.Numerator /= n;
            this.Denominator /= n;
        }

        public Rational(int numerator, int denominator = 1)
        {
            this.Numerator = numerator;
            this.Denominator = denominator;
            if (this.Numerator == 0 && this.Denominator != 0) this.Denominator = 1;
            if (this.Denominator < 0)
            {
                this.Numerator = - numerator;
                this.Denominator = - denominator;
            }
            if (this.Numerator != 0 && this.Denominator != 0) this.Reduce();
        }

        public static Rational operator +(Rational a, Rational b)
        {
            return new Rational(
                a.Numerator * b.Denominator + b.Numerator * a.Denominator, 
                a.Denominator * b.Denominator);
        }

        public static Rational operator -(Rational a, Rational b)
        {
            return new Rational(
                a.Numerator * b.Denominator - b.Numerator * a.Denominator,
                a.Denominator * b.Denominator);
        }

        public static Rational operator *(Rational a, Rational b)
        {
            return new Rational(a.Numerator * b.Numerator, a.Denominator * b.Denominator);
        }

        public static Rational operator /(Rational a, Rational b)
        {
            if (a.Denominator == 0 || b.Denominator == 0) return new Rational(1, 0);
            return a * new Rational(b.Denominator, b.Numerator);
        }

        public static implicit operator double(Rational v)
        {
            return (double)v.Denominator == 0 ?
                double.NaN : (double)v.Numerator / (double)v.Denominator;
        }

        public static implicit operator Rational(int v)
        {
            return new Rational(v);
        }

        public static implicit operator int(Rational v)
        {
            return v.Numerator % v.Denominator == 0 ?
                v.Numerator / v.Denominator : throw new Exception();
        }
    }
}