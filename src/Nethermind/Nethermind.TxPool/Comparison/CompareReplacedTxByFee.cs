//  Copyright (c) 2021 Demerzel Solutions Limited
//  This file is part of the Nethermind library.
// 
//  The Nethermind library is free software: you can redistribute it and/or modify
//  it under the terms of the GNU Lesser General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
// 
//  The Nethermind library is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
//  GNU Lesser General Public License for more details.
// 
//  You should have received a copy of the GNU Lesser General Public License
//  along with the Nethermind. If not, see <http://www.gnu.org/licenses/>.
// 

using System.Collections.Generic;
using Nethermind.Core;
using Nethermind.Int256;

namespace Nethermind.TxPool.Comparison
{
    /// <summary>
    /// Compare fee of newcomer transaction with fee of transaction intended to be replaced increased by given percent
    /// </summary>
    public class CompareReplacedTxByFee : IComparer<Transaction?>
    {
        public static readonly CompareReplacedTxByFee Instance = new();
        
        private CompareReplacedTxByFee() { }
        
        // To replace old transaction, new transaction needs to have fee higher by at least 10% (1/10) of current fee.
        // It is required to avoid acceptance and propagation of transaction with almost the same fee as replaced one.
        private const int PartOfFeeRequiredToIncrease = 10;
        
        public int Compare(Transaction? x, Transaction? y)
        {
            if (ReferenceEquals(x, y)) return 0;
            if (ReferenceEquals(null, y)) return 1;
            if (ReferenceEquals(null, x)) return -1;
            
            // if gas bottleneck was calculated, it's highest priority for sorting
            // if not, different method of sorting by gas price is needed
            if (x.GasBottleneck != 0 || y.GasBottleneck != 0)
            {
                y.GasBottleneck.Divide(PartOfFeeRequiredToIncrease, out UInt256 bumpGasBottleneck);
                return (y.GasBottleneck + bumpGasBottleneck).CompareTo(x.GasBottleneck);
            }
            
            y.GasPrice.Divide(10, out UInt256 bumpGasPrice);
            return (y.GasPrice + bumpGasPrice).CompareTo(x.GasPrice);
        }

    }
}
