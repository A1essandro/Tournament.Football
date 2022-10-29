using System.Security.Cryptography;
using System.Text;
using System.Linq;
using System;
using Tournament.Contracts;

namespace Tournament.Football;

public class Team : IParticipant, IEquatable<Team>
{
    private readonly int _hashcode;

    public Team(string name, double rating)
    {
        Rating = rating;
        Name = name;
        ShortName = string.Join(string.Empty, name.Take(2).Concat(name.Split(' ').Last().Take(1))).ToUpper();
        using (MD5 md5 = MD5.Create())
        {
            md5.Initialize();
            md5.ComputeHash(Encoding.UTF8.GetBytes(name));
            var hash = md5.Hash;
            _hashcode = hash.Aggregate(int.MaxValue, (c, b) => c ^ b);
        }
    }

    public double Rating { get; set; }

    public int Winner { get; set; }

    public string Name { get; }
    public string ShortName { get; }

    public override string ToString() => ShortName;

    public override int GetHashCode() => _hashcode;

    public bool Equals(Team other) => _hashcode == other._hashcode;

}
