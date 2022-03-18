using System;
using System.Collections.Generic;
using System.Text;
using VSSystem.Data.DAL;
using VSSystem.Data.File.Images.DTO;

namespace VSSystem.Data.File.Images.FAL
{
    public class IDataFAL<TPositionObjectInFile, TOutput> : DataDAL<TPositionObjectInFile> where TPositionObjectInFile : PositionObjectInFileDTO
    {
        protected string _fileIDFieldName, _offsetFieldName, _lengthFieldName, _rootPathFieldName, _relativePathFieldName;
        protected string _fileTableName;
        public IDataFAL() : base(Variables.SqlPoolProcess)
        {
            _fileIDFieldName = "File_ID";
            _offsetFieldName = "IndexInFile";
            _lengthFieldName = "DataLength";
            _rootPathFieldName = "RootPath";
            _relativePathFieldName = "RelativePath";
        }
    }
}
