using AvScanLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvScanLibrary
{
    public static class ScanFileLogRepo
    {
        static List<ScanFileLog> _list; // Static List instance

        static ScanFileLogRepo()
        {
            //
            // Allocate the list.
            //
            _list = new List<ScanFileLog>();
        }

        public static void Record(ScanFileLog value)
        {
            //
            // Record this value in the list.
            //
            _list.Add(value);
        }

        public static IEnumerable<ScanFileLog> List
        {
            get {

                return _list.OrderByDescending(q => q.ScanId);
            }
        }

        public static ScanFileLog GetbyId(int id)
        {
            var list = (from q in _list
                   where q.ScanId == id
                   select q).First();

            return list;
        }

        public static int Count()
        {
            //
            // Write out the results.
            //
            return _list.Count();
        }

        public static void Flush()
        {
            //
            // Write out the results.
            //
            _list.Clear();
        }
    }
}
