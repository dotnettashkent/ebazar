using Riok.Mapperly.Abstractions;
using Shared.Features;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Features.Address
{
	[Mapper]
	public static partial class AddressMapper
	{
		#region Usable
		public static AddressView MapToView(this AddressEntity src) => src.To();
		public static List<AddressView> MapToViewList(this List<AddressEntity> src) => src.ToList();
		public static AddressEntity MapFromView(this AddressView src) => src.From();
		#endregion

		#region Internal

		private static partial AddressView To(this AddressEntity src);

		private static partial List<AddressView> ToList(this List<AddressEntity> src);

		private static partial AddressEntity From(this AddressView FileView);

		public static partial void From(AddressView userView, AddressEntity userEntity);
	}
}
