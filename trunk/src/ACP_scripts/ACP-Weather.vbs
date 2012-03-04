' Este archivo debe estar ubicado en el siguiente path\filename: 
' C:\Program Files\ACP Obs Control\ACP-Weather.vbs
'
' Standard ACP Weather Safety script. Please see ACP Help, Customizing ACP, 
' Adding to ACP's Logic, Weather Safety Script. If you have a real dome Or
' if the scope can clear your roll-off roof under all conditions, then 
' you can adjust this so that the dome/roof closes right away
'
Sub Main()
	Console.PrintLine "Weather Safety Script"
	On Error Resume Next
	Telescope.Park
End Sub

