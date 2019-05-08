# DynamicTileSphere
Started as a TileBase RPG idea
Had fun with implimenting and optimizing graphs, as well as dynamically generating a mesh.
Scraped together a demo of the work already made. To run the demo, open the demo scene and play. 

## Controls 
Drag to move the view around, and scroll to zoom.

## Important Notes
While you can edit values from the editor, values modified in the editor are not reflected in the ingame gui.
Rather than recalculate the graph every time it's needed, the graph is saved locally, to the persistant data path. Only the vertex position and the description of the graph is saved.
