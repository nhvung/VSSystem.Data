using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using VSSystem.Data.Filters;
using VSSystem.Data.File.Images.DTO;

namespace VSSystem.Data.File.Images.FAL
{
    public class IDataFAL<TID, TPositionObjectInFile, TOuput> : IDataFAL<TPositionObjectInFile, TOuput> where TPositionObjectInFile : PositionObjectInFileDTO
    {
        protected string _idFieldName;
        public IDataFAL() : base()
        {
            _idFieldName = "ID";
        }
        protected virtual List<TPositionObjectInFile> GetLocations(List<TID> ids)
        {
            try
            {
                if (string.IsNullOrEmpty(_fileTableName)) _fileTableName = _TableName + "File";
                if (ids?.Count > 0)
                {
                    string idFt = BaseFilter.GetFilter("t." + _idFieldName, ids.ToArray());
                    string query = string.Format("select t.{0} as 'ID', t.{1} as 'File_ID', t.{2} as 'Offset', t.{3} as 'Length', tf.{4} as 'RootPath', tf.{5} as 'RelativePath' from {6} t left join {7} tf on t.{1} = tf.{1} where {8}",
                        _idFieldName, _fileIDFieldName, _offsetFieldName, _lengthFieldName, _rootPathFieldName, _relativePathFieldName, _TableName, _fileTableName, idFt);
                    List<TPositionObjectInFile> locations = ExecuteReader<TPositionObjectInFile>(query);
                    return locations;
                }
                return new List<TPositionObjectInFile>();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        protected virtual TPositionObjectInFile GetLocation(TID id)
        {
            try
            {
                if (string.IsNullOrEmpty(_fileTableName)) _fileTableName = _TableName + "File";
                string idFt = BaseFilter.GetFilter("t." + _idFieldName, id);
                string query = string.Format("select t.{0} as 'ID', t.{1} as 'File_ID', t.{2} as 'Offset', t.{3} as 'Length', tf.{4} as 'RootPath', tf.{5} as 'RelativePath' from {6} t left join {7} tf on t.{1} = tf.{1} where {8}",
                    _idFieldName, _fileIDFieldName, _offsetFieldName, _lengthFieldName, _rootPathFieldName, _relativePathFieldName, _TableName, _fileTableName, idFt);
                List<TPositionObjectInFile> locations = ExecuteReader<TPositionObjectInFile>(query);
                return locations.FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public virtual List<TOuput> GetObjects(List<TID> ids, string shareDataFileFolderPath = "")
        {
            try
            {
                return new List<TOuput>();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public virtual TOuput GetObject(TID id, string shareDataFileFolderPath = "")
        {
            return default(TOuput);
        }
        protected byte[] GetDataInFile(TPositionObjectInFile position, string shareDataFileFolderPath = "")
        {

            try
            {
                if (position == null || position.Offset <= 0 || position.Length <= 0) return null;
                byte[] result = new byte[position.Length];
                DirectoryInfo rootFolder = new DirectoryInfo(position.RootPath.Replace("\\", "/"));
                if (!string.IsNullOrEmpty(shareDataFileFolderPath))
                {
                    rootFolder = new DirectoryInfo(shareDataFileFolderPath + "/" + Path.GetFileName(rootFolder.Name));
                }
                using (FileStream fs = new FileStream(rootFolder.FullName + "/" + position.RelativePath.Replace("\\", "/"), FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                {
                    fs.Seek(position.Offset, SeekOrigin.Begin);
                    fs.Read(result, 0, result.Length);
                    fs.Close();
                    fs.Dispose();
                }
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        protected List<byte[]> GetDataInFile(List<TPositionObjectInFile> positions, string shareDataFileFolderPath = "")
        {
            try
            {
                if (positions.Count == 0) return new List<byte[]>();
                List<byte[]> result = new List<byte[]>();
                var posGrp = positions.GroupBy(ite => new { RootPath = new DirectoryInfo(ite.RootPath.ToUpper()).FullName, RelativePath = ite.RelativePath.ToUpper() }).ToArray();
                foreach (var grp in posGrp)
                {
                    DirectoryInfo rootFolder = new DirectoryInfo(grp.Key.RootPath.Replace("\\", "/"));
                    if (!rootFolder.Exists && !string.IsNullOrEmpty(shareDataFileFolderPath))
                    {
                        rootFolder = new DirectoryInfo(shareDataFileFolderPath + "/" + rootFolder.Name);
                    }
                    using (FileStream fs = new FileStream(rootFolder.FullName + "/" + grp.Key.RelativePath.Replace("\\", "/"), FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                    {
                        foreach (var position in grp)
                        {
                            if (position.Offset <= 0 || position.Length <= 0) continue;
                            byte[] buff = new byte[position.Length];
                            fs.Seek(position.Offset, SeekOrigin.Begin);
                            fs.Read(buff, 0, buff.Length);
                            result.Add(buff);
                        }
                        fs.Close();
                        fs.Dispose();
                    }
                }
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        protected List<KeyValuePair<TID, byte[]>> GetDataInFile(List<TPositionObjectInFile> positions, Func<TPositionObjectInFile, TID> idSelector, string shareDataFileFolderPath = "")
        {
            try
            {
                if (positions.Count == 0 || idSelector == null) return new List<KeyValuePair<TID, byte[]>>();
                List<KeyValuePair<TID, byte[]>> result = new List<KeyValuePair<TID, byte[]>>();
                var posGrp = positions.GroupBy(ite => new { RootPath = new DirectoryInfo(ite.RootPath.ToUpper()).FullName, RelativePath = ite.RelativePath.ToUpper() }).ToArray();
                foreach (var grp in posGrp)
                {
                    DirectoryInfo rootFolder = new DirectoryInfo(grp.Key.RootPath.Replace("\\", "/"));
                    if (!rootFolder.Exists && !string.IsNullOrEmpty(shareDataFileFolderPath))
                    {
                        rootFolder = new DirectoryInfo(shareDataFileFolderPath + "/" + rootFolder.Name);
                    }
                    using (FileStream fs = new FileStream(rootFolder.FullName + "/" + grp.Key.RelativePath.Replace("\\", "/"), FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                    {
                        foreach (var position in grp)
                        {
                            if (position.Offset <= 0 || position.Length <= 0) continue;
                            TID id = idSelector.Invoke(position);
                            byte[] buff = new byte[position.Length];
                            fs.Seek(position.Offset, SeekOrigin.Begin);
                            fs.Read(buff, 0, buff.Length);
                            result.Add(new KeyValuePair<TID, byte[]>(id, buff));
                        }
                        fs.Close();
                        fs.Dispose();
                    }
                }
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected List<KeyValuePair<TNewID, byte[]>> GetDataInFile<TNewID>(List<TPositionObjectInFile> positions, Func<TPositionObjectInFile, TNewID> idSelector, string shareDataFileFolderPath = "")
        {
            try
            {
                if (positions.Count == 0 || idSelector == null) return new List<KeyValuePair<TNewID, byte[]>>();
                List<KeyValuePair<TNewID, byte[]>> result = new List<KeyValuePair<TNewID, byte[]>>();
                var posGrp = positions.GroupBy(ite => new { RootPath = new DirectoryInfo(ite.RootPath.ToUpper()).FullName, RelativePath = ite.RelativePath.ToUpper() }).ToArray();
                foreach (var grp in posGrp)
                {
                    DirectoryInfo rootFolder = new DirectoryInfo(grp.Key.RootPath.Replace("\\", "/"));
                    if (!rootFolder.Exists && !string.IsNullOrEmpty(shareDataFileFolderPath))
                    {
                        rootFolder = new DirectoryInfo(shareDataFileFolderPath + "/" + rootFolder.Name);
                    }
                    using (FileStream fs = new FileStream(rootFolder.FullName + "/" + grp.Key.RelativePath.Replace("\\", "/"), FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                    {
                        foreach (var position in grp)
                        {
                            if (position.Offset <= 0 || position.Length <= 0) continue;
                            TNewID id = idSelector.Invoke(position);
                            byte[] buff = new byte[position.Length];
                            fs.Seek(position.Offset, SeekOrigin.Begin);
                            fs.Read(buff, 0, buff.Length);
                            result.Add(new KeyValuePair<TNewID, byte[]>(id, buff));
                        }
                        fs.Close();
                        fs.Dispose();
                    }
                }
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
