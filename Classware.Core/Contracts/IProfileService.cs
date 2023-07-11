using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Classware.Core.Contracts
{
	public interface IProfileService
	{
		Task UploadPictureAsync(byte[] data,string userId);

		Task EditProfileInformationAsync(string userId, byte[]? data,string? firstName,string? middleName,string? lastName,int? age, string? gender);
	}
}
