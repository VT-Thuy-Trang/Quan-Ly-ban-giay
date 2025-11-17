using Oracle.ManagedDataAccess.Client;
using System;
using System.Data;

namespace QL_GiayTT.Class
{
    public static class OracleSession
    {
        private static KetNoi _ketNoi;

        public static void Initialize(KetNoi ketNoi)
        {
            _ketNoi = ketNoi ?? throw new ArgumentNullException(nameof(ketNoi));
        }

        public static KetNoi CurrentKetNoi
        {
            get
            {
                if (_ketNoi == null)
                    throw new InvalidOperationException("Oracle session has not been initialized.");
                return _ketNoi;
            }
        }

        public static OracleConnection Connection
        {
            get
            {
                if (_ketNoi == null)
                    throw new InvalidOperationException("Oracle session has not been initialized.");

                var connection = _ketNoi.GetConnection();
                if (connection == null)
                    throw new InvalidOperationException("Oracle connection is not available.");

                if (connection.State == ConnectionState.Closed)
                    connection.Open();

                return connection;
            }
        }
    }
}

