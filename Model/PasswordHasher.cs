using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ExamifyX.Model
{
	//this class hashes the password in the database and decrypts it upon log in
	public static class PasswordHasher
	{
		public static byte[] GenerateSalt(int size)
		{
			byte[] salt = new byte[size];
			using (var rng = RandomNumberGenerator.Create())
			{
				rng.GetBytes(salt);
			}
			return salt;
		}
		public static string HashPassword(string password)
		{
			if(password == null)
			{
				throw new ArgumentNullException(nameof(password));
			}

			byte[] salt = GenerateSalt(16);

			byte[] buffer2;
			using (Rfc2898DeriveBytes bytes = new Rfc2898DeriveBytes(password, salt, 10000, HashAlgorithmName.SHA256))
			{
				buffer2 = bytes.GetBytes(20);
			}

			byte[] hashBytes = new byte[36];
			Array.Copy(salt, 0, hashBytes, 0, 16);
			Array.Copy(buffer2, 0, hashBytes, 16, 20);

			return Convert.ToBase64String(hashBytes);
		}

		//Verifies a password against a hash
		public static bool VerifyPassword(string hashedPassword, string plainPassword)
		{
			//Extract the bytes
			byte[] hashBytes = Convert.FromBase64String(hashedPassword);

			//get the salt
			byte[] salt = new byte[16];
			Array.Copy(hashBytes, 0, salt, 0, 16);

			//Compute the has on the password the user entered
			var hashGenerator = new Rfc2898DeriveBytes(plainPassword, salt, 10000, HashAlgorithmName.SHA256);
			byte[] hash = hashGenerator.GetBytes(20);

			for(int i = 0; i<20;  i++)
			{
				if (hashBytes[i + 16] != hash[i])
				{
					return false; // password does not match
				}
			}

			return true; // password matches
		}
	}
}
