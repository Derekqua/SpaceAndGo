
@foreach (var Object in ViewBag.Object)
{
    if (@Object.Location=="Block 1 ")
    {
        if (@Object.CrowdNow <= 50)
        {
    document.getElementById("blk1").style.fill = "green";
        }
        else if (@Object.CrowdNow <= 100)
        {
    document.getElementById("blk1").style.fill = "orange";
        }
        else
        {
    document.getElementById("blk1").style.fill = "red";
        }
    }
}