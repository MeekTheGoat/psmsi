#Requires -Version 2.0

function Get-MSISharedComponentInfo
{
# .ExternalHelp Microsoft.WindowsInstaller.PowerShell.dll-Help.xml

	[CmdletBinding()]
	param
	(
		[Parameter(Position = 0)]
		[ValidateNotNullOrEmpty()]
		[Microsoft.WindowsInstaller.PowerShell.ValidateGuid()]
		[string[]] $ComponentCode,
		
		[Parameter(Position = 1)]
		[ValidateRange(2, 2147483647)]
		[int] $Count = 2
	)
	
	end
	{
		$getcomponents = { get-msicomponentinfo }
		if ($ComponentCode)
		{
			$getcomponents = { get-msicomponentinfo -componentcode $ComponentCode }
		}
		& $getcomponents | group-object -property ComponentCode | where-object { $_.Count -ge $Count } `
			| select-object -expand Group
	}
}

export-modulemember -cmdlet * -function *

# Update the usage information for this module if installed.
[Microsoft.WindowsInstaller.PowerShell.Module]::Use()
