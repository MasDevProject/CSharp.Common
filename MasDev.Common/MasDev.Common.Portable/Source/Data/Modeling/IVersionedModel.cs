using MasDev.Common.Modeling;
using System;


namespace MasDev.Common.Modeling
{
	public interface IVersionedModel : IUndeletableModel
	{
	}





	public interface IModelVersioning : IModel
	{
		DateTime? CreationUTC{ get; set; }
	}





	public interface IVersionedModel<TModelVersioning> : IVersionedModel where TModelVersioning : IModelVersioning
	{
		TModelVersioning CurrentVersion { get; set; }
	}





	public interface IModelVersioning<TVersionedModel> : IModelVersioning where TVersionedModel : IVersionedModel
	{
		TVersionedModel Parent { get; set; }
	}
}

