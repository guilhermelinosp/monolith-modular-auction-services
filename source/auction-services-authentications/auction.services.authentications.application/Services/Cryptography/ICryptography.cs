namespace auction.services.authentications.application.Services.Cryptography;

public interface ICryptography
{
	string EncryptPassword(string password);
	bool VerifyPassword(string password, string hashedPassword);
}