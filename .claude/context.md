# Claude Session Context

## Project
- Unity project: LunyScratch Examples (actually: LunyScript Examples)
- Package: de.codesmile.lunyscript (submodule)
- Branch: main
- Cross-Engine: Luny/LunyScript execute in Unity, Godot, other C# engines
- Package location: Packages/de.codesmile.lunyscript
  - 'Luny/' contains general-purpose (not just LunyScript) engine-agnostic types
  - 'Luny.Unity/' contains Unity-specific implementations/bindings
  - 'LunyScript/' contains engine-agnostic scripting types, utilizing Luny library
  - 'LunyScript.Unity/' contains Unity-specific bindings for LunyScript
  - analogous Luny.Godot and LunyScript.Godot exist in the Godot addon repository

## Implementation Notes
- for logging, prefer LunyLogger methods (it redirects to engine-native logging, or in absence to Console.WriteLine)

## Preferences
- End all responses with: `#Credits spent in chat: ~$X.XX (+$X.XX), Context: XX% budget` (credits = high estimate, accumulative for entire chat; in brackets: by how much value changed since last prompt)
- Concise, direct responses
- Ask if a new preference from prompts should be added to context.md

## Notes
- Test scene: in package
