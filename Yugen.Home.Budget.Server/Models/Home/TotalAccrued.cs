using MudBlazor;
using System;

namespace Yugen.Home.Budget.Server.Models.Home;

public class TotalAccrued
{
	public TotalAccrued(
		decimal current,
		decimal compareTo)
	{
		Current = current;
		CompareTo = compareTo;
		Percentage = Percent(current, compareTo);

		Icon = Percentage switch
		{
			> 0 => Icons.Material.Filled.ArrowUpward,
			< 0 => Icons.Material.Filled.ArrowDownward,
			_ => Icons.Material.Filled.ArrowRight
		};
	}

	public TotalAccrued(decimal current)
	{
		Current = current;
		CompareTo = 0;
		Percentage = 0;

		Icon = Percentage switch
		{
			> 0 => Icons.Material.Filled.ArrowUpward,
			< 0 => Icons.Material.Filled.ArrowDownward,
			_ => Icons.Material.Filled.ArrowRight
		};
	}

	public decimal CompareTo { get; set; }

	public decimal Current { get; set; }

	public string Icon { get; set; }

	public decimal Percentage { get; set; }

	private static decimal Percent(decimal current, decimal compareTo)
	{
		if (current == 0)
		{
			current = 1;
		}
		if (compareTo == 0)
		{
			compareTo = 1;
		}
		return decimal.Round((current - compareTo) / Math.Abs(compareTo) * 100, 2);
	}
}