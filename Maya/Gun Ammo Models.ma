//Maya ASCII 2020 scene
//Name: Gun Ammo Models.ma
//Last modified: Thu, Apr 29, 2021 09:14:36 PM
//Codeset: 1252
requires maya "2020";
currentUnit -l centimeter -a degree -t film;
fileInfo "application" "maya";
fileInfo "product" "Maya 2020";
fileInfo "version" "2020";
fileInfo "cutIdentifier" "201911140446-42a737a01c";
fileInfo "osv" "Microsoft Windows 10 Technical Preview  (Build 19042)\n";
fileInfo "UUID" "6493BA1A-49F1-FAE4-5102-1BA9B12EBD05";
createNode transform -s -n "persp";
	rename -uid "BCBEF6D2-4D5B-F81C-60F2-E8B67F55A2CA";
	setAttr ".v" no;
	setAttr ".t" -type "double3" 51.252365645453111 23.514820770121851 32.564473953982819 ;
	setAttr ".r" -type "double3" -21.938352728637124 418.19999999947566 0 ;
createNode camera -s -n "perspShape" -p "persp";
	rename -uid "16D02C0C-44CF-0A5D-93F1-C38D76E984D8";
	setAttr -k off ".v" no;
	setAttr ".fl" 34.999999999999993;
	setAttr ".coi" 64.927169145127152;
	setAttr ".imn" -type "string" "persp";
	setAttr ".den" -type "string" "persp_depth";
	setAttr ".man" -type "string" "persp_mask";
	setAttr ".hc" -type "string" "viewSet -p %camera";
createNode transform -s -n "top";
	rename -uid "2BA5F5AB-473B-0FA7-ECD7-DBA70835E5F1";
	setAttr ".v" no;
	setAttr ".t" -type "double3" 0 1000.1 0 ;
	setAttr ".r" -type "double3" -90 0 0 ;
createNode camera -s -n "topShape" -p "top";
	rename -uid "CE282AEE-4298-298A-1A40-459BAAB7A0F6";
	setAttr -k off ".v" no;
	setAttr ".rnd" no;
	setAttr ".coi" 1000.1;
	setAttr ".ow" 30;
	setAttr ".imn" -type "string" "top";
	setAttr ".den" -type "string" "top_depth";
	setAttr ".man" -type "string" "top_mask";
	setAttr ".hc" -type "string" "viewSet -t %camera";
	setAttr ".o" yes;
createNode transform -s -n "front";
	rename -uid "C9CA14DA-4CC5-EBD9-9B54-918F45339A39";
	setAttr ".v" no;
	setAttr ".t" -type "double3" 0 0 1000.1 ;
createNode camera -s -n "frontShape" -p "front";
	rename -uid "CC1ACEA7-4119-40A2-9EC9-B1898F3136AB";
	setAttr -k off ".v" no;
	setAttr ".rnd" no;
	setAttr ".coi" 1000.1;
	setAttr ".ow" 30;
	setAttr ".imn" -type "string" "front";
	setAttr ".den" -type "string" "front_depth";
	setAttr ".man" -type "string" "front_mask";
	setAttr ".hc" -type "string" "viewSet -f %camera";
	setAttr ".o" yes;
createNode transform -s -n "side";
	rename -uid "4D8AD08A-47FD-6B56-09A1-B5AB31A4277A";
	setAttr ".v" no;
	setAttr ".t" -type "double3" 1000.1 0 0 ;
	setAttr ".r" -type "double3" 0 90 0 ;
createNode camera -s -n "sideShape" -p "side";
	rename -uid "A67D3AAB-4FAB-A78C-715F-488B177F528D";
	setAttr -k off ".v" no;
	setAttr ".rnd" no;
	setAttr ".coi" 1000.1;
	setAttr ".ow" 30;
	setAttr ".imn" -type "string" "side";
	setAttr ".den" -type "string" "side_depth";
	setAttr ".man" -type "string" "side_mask";
	setAttr ".hc" -type "string" "viewSet -s %camera";
	setAttr ".o" yes;
createNode transform -n "Hardlight_Bullet";
	rename -uid "B9858528-4CEC-705B-0F76-249FFC41624F";
	setAttr ".v" no;
createNode mesh -n "Hardlight_BulletShape" -p "Hardlight_Bullet";
	rename -uid "20CAE097-496E-A253-F07D-CE878384ADEF";
	setAttr -k off ".v";
	setAttr ".vir" yes;
	setAttr ".vif" yes;
	setAttr ".pv" -type "double2" 0.5 0.5 ;
	setAttr ".uvst[0].uvsn" -type "string" "map1";
	setAttr ".cuvs" -type "string" "map1";
	setAttr ".dcc" -type "string" "Ambient+Diffuse";
	setAttr ".covm[0]"  0 1 1;
	setAttr ".cdvm[0]"  0 1 1;
createNode transform -n "Impact_Missile";
	rename -uid "9B9998B1-46B6-A8ED-E043-439D4BE6E1E2";
	setAttr ".rp" -type "double3" 4.4703483581542969e-08 -4.4703483581542969e-08 0.71323919296264648 ;
	setAttr ".sp" -type "double3" 4.4703483581542969e-08 -4.4703483581542969e-08 0.71323919296264648 ;
createNode mesh -n "Impact_MissileShape" -p "Impact_Missile";
	rename -uid "33719800-42B0-7EFF-A711-E2A2D3989681";
	setAttr -k off ".v";
	setAttr ".vir" yes;
	setAttr ".vif" yes;
	setAttr ".pv" -type "double2" 0.5 0.54746240377426147 ;
	setAttr ".uvst[0].uvsn" -type "string" "map1";
	setAttr ".cuvs" -type "string" "map1";
	setAttr ".dcc" -type "string" "Ambient+Diffuse";
	setAttr ".covm[0]"  0 1 1;
	setAttr ".cdvm[0]"  0 1 1;
createNode lightLinker -s -n "lightLinker1";
	rename -uid "C1191B0C-4E0A-CFE7-98AC-939D3C91696F";
	setAttr -s 2 ".lnk";
	setAttr -s 2 ".slnk";
createNode shapeEditorManager -n "shapeEditorManager";
	rename -uid "F8BBF955-4E90-118C-98CE-DAB25B7E8632";
createNode poseInterpolatorManager -n "poseInterpolatorManager";
	rename -uid "38F33A40-472E-A61A-2BBA-4EB569AED266";
createNode displayLayerManager -n "layerManager";
	rename -uid "9C0B8C60-4F72-318C-C8F4-E6B7A0F5E277";
createNode displayLayer -n "defaultLayer";
	rename -uid "10E41F7E-4800-2A8E-10C9-41BCA2A575CA";
createNode renderLayerManager -n "renderLayerManager";
	rename -uid "C56217E7-46A1-05AA-4D42-09B6E591F587";
createNode renderLayer -n "defaultRenderLayer";
	rename -uid "576434CB-4CA3-9929-76C5-C18F933B8F88";
	setAttr ".g" yes;
createNode polyCube -n "polyCube1";
	rename -uid "0DACB6B0-4CB9-BA09-5C22-988BDA622A68";
	setAttr ".cuv" 4;
createNode polyExtrudeFace -n "polyExtrudeFace1";
	rename -uid "146CE973-49B1-5B2C-9E56-F8AC53C071C5";
	setAttr ".ics" -type "componentList" 2 "f[0]" "f[2]";
	setAttr ".ix" -type "matrix" 0.70710678118654746 -0.70710678118654768 0 0 0.70710678118654768 0.70710678118654746 0 0
		 0 0 1 0 0 0 0 1;
	setAttr ".ws" yes;
	setAttr ".pvt" -type "float3" 0 0 2.9802322e-08 ;
	setAttr ".rs" 40435;
	setAttr ".lt" -type "double3" 0 0 0.48923807038519707 ;
	setAttr ".c[0]"  0 1 1;
	setAttr ".cbn" -type "double3" -0.70710678118654757 -0.70710678118654757 -0.5 ;
	setAttr ".cbx" -type "double3" 0.70710678118654757 0.70710678118654757 0.50000005960464478 ;
createNode polyMergeVert -n "polyMergeVert1";
	rename -uid "6CA0FB9C-4711-F7CE-A199-AFBBF2AB62D0";
	setAttr ".ics" -type "componentList" 1 "vtx[8:11]";
	setAttr ".ix" -type "matrix" 0.70710678118654746 -0.70710678118654768 0 0 0.70710678118654768 0.70710678118654746 0 0
		 0 0 1 0 0 0 0 1;
	setAttr ".d" 1e-06;
createNode polyTweak -n "polyTweak1";
	rename -uid "18150A3F-4DA0-B7D4-5E68-568743FFA36D";
	setAttr ".uopa" yes;
	setAttr -s 14 ".tk";
	setAttr ".tk[0]" -type "float3" 0 0 5.9604645e-08 ;
	setAttr ".tk[1]" -type "float3" 0 0 5.9604645e-08 ;
	setAttr ".tk[2]" -type "float3" 0 0 5.9604645e-08 ;
	setAttr ".tk[3]" -type "float3" 0 0 5.9604645e-08 ;
	setAttr ".tk[8]" -type "float3" 0.49999997 0.49999997 0 ;
	setAttr ".tk[9]" -type "float3" -0.49999997 0.49999997 0 ;
	setAttr ".tk[10]" -type "float3" -0.49999997 -0.49999997 0 ;
	setAttr ".tk[11]" -type "float3" 0.49999997 -0.49999997 0 ;
	setAttr ".tk[16]" -type "float3" 1.4901161e-08 0 0 ;
	setAttr ".tk[17]" -type "float3" 1.4901161e-08 0 0 ;
createNode polyMergeVert -n "polyMergeVert2";
	rename -uid "2ACF19FF-4E00-C060-33E7-78859C04D02A";
	setAttr ".ics" -type "componentList" 1 "vtx[9:12]";
	setAttr ".ix" -type "matrix" 0.70710678118654746 -0.70710678118654768 0 0 0.70710678118654768 0.70710678118654746 0 0
		 0 0 1 0 0 0 0 1;
	setAttr ".d" 1e-06;
createNode polyTweak -n "polyTweak2";
	rename -uid "F31078B2-47CA-62AD-3285-20A6C898EE02";
	setAttr ".uopa" yes;
	setAttr -s 4 ".tk[9:12]" -type "float3"  0.49999997 -0.49999997 0 -0.49999997
		 -0.49999997 0 -0.49999997 0.49999997 0 0.49999997 0.49999997 0;
createNode polySoftEdge -n "polySoftEdge1";
	rename -uid "95F64152-454E-E416-F830-8EA364B6ECDF";
	setAttr ".uopa" yes;
	setAttr ".ics" -type "componentList" 1 "e[*]";
	setAttr ".ix" -type "matrix" 0.70710678118654746 -0.70710678118654768 0 0 0.70710678118654768 0.70710678118654746 0 0
		 0 0 1 0 0 0 0 1;
	setAttr ".a" 180;
createNode polyTweak -n "polyTweak3";
	rename -uid "CB1F14E7-4B99-BBEF-38C6-7F995D07E5AF";
	setAttr ".uopa" yes;
	setAttr -s 8 ".tk[0:7]" -type "float3"  0 0 -0.15316786 0 0 -0.15316786
		 0 0 -0.15316786 0 0 -0.15316786 0 0 0.15316786 0 0 0.15316786 0 0 0.15316786 0 0
		 0.15316786;
createNode script -n "uiConfigurationScriptNode";
	rename -uid "AF7E6EE9-4729-3768-588E-F0B6EB3DA3EF";
	setAttr ".b" -type "string" (
		"// Maya Mel UI Configuration File.\n//\n//  This script is machine generated.  Edit at your own risk.\n//\n//\n\nglobal string $gMainPane;\nif (`paneLayout -exists $gMainPane`) {\n\n\tglobal int $gUseScenePanelConfig;\n\tint    $useSceneConfig = $gUseScenePanelConfig;\n\tint    $nodeEditorPanelVisible = stringArrayContains(\"nodeEditorPanel1\", `getPanel -vis`);\n\tint    $nodeEditorWorkspaceControlOpen = (`workspaceControl -exists nodeEditorPanel1Window` && `workspaceControl -q -visible nodeEditorPanel1Window`);\n\tint    $menusOkayInPanels = `optionVar -q allowMenusInPanels`;\n\tint    $nVisPanes = `paneLayout -q -nvp $gMainPane`;\n\tint    $nPanes = 0;\n\tstring $editorName;\n\tstring $panelName;\n\tstring $itemFilterName;\n\tstring $panelConfig;\n\n\t//\n\t//  get current state of the UI\n\t//\n\tsceneUIReplacement -update $gMainPane;\n\n\t$panelName = `sceneUIReplacement -getNextPanel \"modelPanel\" (localizedPanelLabel(\"Top View\")) `;\n\tif (\"\" != $panelName) {\n\t\t$label = `panel -q -label $panelName`;\n\t\tmodelPanel -edit -l (localizedPanelLabel(\"Top View\")) -mbv $menusOkayInPanels  $panelName;\n"
		+ "\t\t$editorName = $panelName;\n        modelEditor -e \n            -camera \"top\" \n            -useInteractiveMode 0\n            -displayLights \"default\" \n            -displayAppearance \"smoothShaded\" \n            -activeOnly 0\n            -ignorePanZoom 0\n            -wireframeOnShaded 0\n            -headsUpDisplay 1\n            -holdOuts 1\n            -selectionHiliteDisplay 1\n            -useDefaultMaterial 0\n            -bufferMode \"double\" \n            -twoSidedLighting 0\n            -backfaceCulling 0\n            -xray 0\n            -jointXray 0\n            -activeComponentsXray 0\n            -displayTextures 0\n            -smoothWireframe 0\n            -lineWidth 1\n            -textureAnisotropic 0\n            -textureHilight 1\n            -textureSampling 2\n            -textureDisplay \"modulate\" \n            -textureMaxSize 32768\n            -fogging 0\n            -fogSource \"fragment\" \n            -fogMode \"linear\" \n            -fogStart 0\n            -fogEnd 100\n            -fogDensity 0.1\n            -fogColor 0.5 0.5 0.5 1 \n"
		+ "            -depthOfFieldPreview 1\n            -maxConstantTransparency 1\n            -rendererName \"vp2Renderer\" \n            -objectFilterShowInHUD 1\n            -isFiltered 0\n            -colorResolution 256 256 \n            -bumpResolution 512 512 \n            -textureCompression 0\n            -transparencyAlgorithm \"frontAndBackCull\" \n            -transpInShadows 0\n            -cullingOverride \"none\" \n            -lowQualityLighting 0\n            -maximumNumHardwareLights 1\n            -occlusionCulling 0\n            -shadingModel 0\n            -useBaseRenderer 0\n            -useReducedRenderer 0\n            -smallObjectCulling 0\n            -smallObjectThreshold -1 \n            -interactiveDisableShadows 0\n            -interactiveBackFaceCull 0\n            -sortTransparent 1\n            -controllers 1\n            -nurbsCurves 1\n            -nurbsSurfaces 1\n            -polymeshes 1\n            -subdivSurfaces 1\n            -planes 1\n            -lights 1\n            -cameras 1\n            -controlVertices 1\n"
		+ "            -hulls 1\n            -grid 1\n            -imagePlane 1\n            -joints 1\n            -ikHandles 1\n            -deformers 1\n            -dynamics 1\n            -particleInstancers 1\n            -fluids 1\n            -hairSystems 1\n            -follicles 1\n            -nCloths 1\n            -nParticles 1\n            -nRigids 1\n            -dynamicConstraints 1\n            -locators 1\n            -manipulators 1\n            -pluginShapes 1\n            -dimensions 1\n            -handles 1\n            -pivots 1\n            -textures 1\n            -strokes 1\n            -motionTrails 1\n            -clipGhosts 1\n            -greasePencils 1\n            -shadows 0\n            -captureSequenceNumber -1\n            -width 1\n            -height 1\n            -sceneRenderFilter 0\n            $editorName;\n        modelEditor -e -viewSelected 0 $editorName;\n        modelEditor -e \n            -pluginObjects \"gpuCacheDisplayFilter\" 1 \n            $editorName;\n\t\tif (!$useSceneConfig) {\n\t\t\tpanel -e -l $label $panelName;\n"
		+ "\t\t}\n\t}\n\n\n\t$panelName = `sceneUIReplacement -getNextPanel \"modelPanel\" (localizedPanelLabel(\"Side View\")) `;\n\tif (\"\" != $panelName) {\n\t\t$label = `panel -q -label $panelName`;\n\t\tmodelPanel -edit -l (localizedPanelLabel(\"Side View\")) -mbv $menusOkayInPanels  $panelName;\n\t\t$editorName = $panelName;\n        modelEditor -e \n            -camera \"side\" \n            -useInteractiveMode 0\n            -displayLights \"default\" \n            -displayAppearance \"smoothShaded\" \n            -activeOnly 0\n            -ignorePanZoom 0\n            -wireframeOnShaded 0\n            -headsUpDisplay 1\n            -holdOuts 1\n            -selectionHiliteDisplay 1\n            -useDefaultMaterial 0\n            -bufferMode \"double\" \n            -twoSidedLighting 0\n            -backfaceCulling 0\n            -xray 0\n            -jointXray 0\n            -activeComponentsXray 0\n            -displayTextures 0\n            -smoothWireframe 0\n            -lineWidth 1\n            -textureAnisotropic 0\n            -textureHilight 1\n            -textureSampling 2\n"
		+ "            -textureDisplay \"modulate\" \n            -textureMaxSize 32768\n            -fogging 0\n            -fogSource \"fragment\" \n            -fogMode \"linear\" \n            -fogStart 0\n            -fogEnd 100\n            -fogDensity 0.1\n            -fogColor 0.5 0.5 0.5 1 \n            -depthOfFieldPreview 1\n            -maxConstantTransparency 1\n            -rendererName \"vp2Renderer\" \n            -objectFilterShowInHUD 1\n            -isFiltered 0\n            -colorResolution 256 256 \n            -bumpResolution 512 512 \n            -textureCompression 0\n            -transparencyAlgorithm \"frontAndBackCull\" \n            -transpInShadows 0\n            -cullingOverride \"none\" \n            -lowQualityLighting 0\n            -maximumNumHardwareLights 1\n            -occlusionCulling 0\n            -shadingModel 0\n            -useBaseRenderer 0\n            -useReducedRenderer 0\n            -smallObjectCulling 0\n            -smallObjectThreshold -1 \n            -interactiveDisableShadows 0\n            -interactiveBackFaceCull 0\n"
		+ "            -sortTransparent 1\n            -controllers 1\n            -nurbsCurves 1\n            -nurbsSurfaces 1\n            -polymeshes 1\n            -subdivSurfaces 1\n            -planes 1\n            -lights 1\n            -cameras 1\n            -controlVertices 1\n            -hulls 1\n            -grid 1\n            -imagePlane 1\n            -joints 1\n            -ikHandles 1\n            -deformers 1\n            -dynamics 1\n            -particleInstancers 1\n            -fluids 1\n            -hairSystems 1\n            -follicles 1\n            -nCloths 1\n            -nParticles 1\n            -nRigids 1\n            -dynamicConstraints 1\n            -locators 1\n            -manipulators 1\n            -pluginShapes 1\n            -dimensions 1\n            -handles 1\n            -pivots 1\n            -textures 1\n            -strokes 1\n            -motionTrails 1\n            -clipGhosts 1\n            -greasePencils 1\n            -shadows 0\n            -captureSequenceNumber -1\n            -width 1\n            -height 1\n"
		+ "            -sceneRenderFilter 0\n            $editorName;\n        modelEditor -e -viewSelected 0 $editorName;\n        modelEditor -e \n            -pluginObjects \"gpuCacheDisplayFilter\" 1 \n            $editorName;\n\t\tif (!$useSceneConfig) {\n\t\t\tpanel -e -l $label $panelName;\n\t\t}\n\t}\n\n\n\t$panelName = `sceneUIReplacement -getNextPanel \"modelPanel\" (localizedPanelLabel(\"Front View\")) `;\n\tif (\"\" != $panelName) {\n\t\t$label = `panel -q -label $panelName`;\n\t\tmodelPanel -edit -l (localizedPanelLabel(\"Front View\")) -mbv $menusOkayInPanels  $panelName;\n\t\t$editorName = $panelName;\n        modelEditor -e \n            -camera \"front\" \n            -useInteractiveMode 0\n            -displayLights \"default\" \n            -displayAppearance \"smoothShaded\" \n            -activeOnly 0\n            -ignorePanZoom 0\n            -wireframeOnShaded 0\n            -headsUpDisplay 1\n            -holdOuts 1\n            -selectionHiliteDisplay 1\n            -useDefaultMaterial 0\n            -bufferMode \"double\" \n            -twoSidedLighting 0\n            -backfaceCulling 0\n"
		+ "            -xray 0\n            -jointXray 0\n            -activeComponentsXray 0\n            -displayTextures 0\n            -smoothWireframe 0\n            -lineWidth 1\n            -textureAnisotropic 0\n            -textureHilight 1\n            -textureSampling 2\n            -textureDisplay \"modulate\" \n            -textureMaxSize 32768\n            -fogging 0\n            -fogSource \"fragment\" \n            -fogMode \"linear\" \n            -fogStart 0\n            -fogEnd 100\n            -fogDensity 0.1\n            -fogColor 0.5 0.5 0.5 1 \n            -depthOfFieldPreview 1\n            -maxConstantTransparency 1\n            -rendererName \"vp2Renderer\" \n            -objectFilterShowInHUD 1\n            -isFiltered 0\n            -colorResolution 256 256 \n            -bumpResolution 512 512 \n            -textureCompression 0\n            -transparencyAlgorithm \"frontAndBackCull\" \n            -transpInShadows 0\n            -cullingOverride \"none\" \n            -lowQualityLighting 0\n            -maximumNumHardwareLights 1\n            -occlusionCulling 0\n"
		+ "            -shadingModel 0\n            -useBaseRenderer 0\n            -useReducedRenderer 0\n            -smallObjectCulling 0\n            -smallObjectThreshold -1 \n            -interactiveDisableShadows 0\n            -interactiveBackFaceCull 0\n            -sortTransparent 1\n            -controllers 1\n            -nurbsCurves 1\n            -nurbsSurfaces 1\n            -polymeshes 1\n            -subdivSurfaces 1\n            -planes 1\n            -lights 1\n            -cameras 1\n            -controlVertices 1\n            -hulls 1\n            -grid 1\n            -imagePlane 1\n            -joints 1\n            -ikHandles 1\n            -deformers 1\n            -dynamics 1\n            -particleInstancers 1\n            -fluids 1\n            -hairSystems 1\n            -follicles 1\n            -nCloths 1\n            -nParticles 1\n            -nRigids 1\n            -dynamicConstraints 1\n            -locators 1\n            -manipulators 1\n            -pluginShapes 1\n            -dimensions 1\n            -handles 1\n            -pivots 1\n"
		+ "            -textures 1\n            -strokes 1\n            -motionTrails 1\n            -clipGhosts 1\n            -greasePencils 1\n            -shadows 0\n            -captureSequenceNumber -1\n            -width 1\n            -height 1\n            -sceneRenderFilter 0\n            $editorName;\n        modelEditor -e -viewSelected 0 $editorName;\n        modelEditor -e \n            -pluginObjects \"gpuCacheDisplayFilter\" 1 \n            $editorName;\n\t\tif (!$useSceneConfig) {\n\t\t\tpanel -e -l $label $panelName;\n\t\t}\n\t}\n\n\n\t$panelName = `sceneUIReplacement -getNextPanel \"modelPanel\" (localizedPanelLabel(\"Persp View\")) `;\n\tif (\"\" != $panelName) {\n\t\t$label = `panel -q -label $panelName`;\n\t\tmodelPanel -edit -l (localizedPanelLabel(\"Persp View\")) -mbv $menusOkayInPanels  $panelName;\n\t\t$editorName = $panelName;\n        modelEditor -e \n            -camera \"persp\" \n            -useInteractiveMode 0\n            -displayLights \"default\" \n            -displayAppearance \"smoothShaded\" \n            -activeOnly 0\n            -ignorePanZoom 0\n"
		+ "            -wireframeOnShaded 0\n            -headsUpDisplay 1\n            -holdOuts 1\n            -selectionHiliteDisplay 1\n            -useDefaultMaterial 0\n            -bufferMode \"double\" \n            -twoSidedLighting 0\n            -backfaceCulling 0\n            -xray 0\n            -jointXray 0\n            -activeComponentsXray 0\n            -displayTextures 0\n            -smoothWireframe 0\n            -lineWidth 1\n            -textureAnisotropic 0\n            -textureHilight 1\n            -textureSampling 2\n            -textureDisplay \"modulate\" \n            -textureMaxSize 32768\n            -fogging 0\n            -fogSource \"fragment\" \n            -fogMode \"linear\" \n            -fogStart 0\n            -fogEnd 100\n            -fogDensity 0.1\n            -fogColor 0.5 0.5 0.5 1 \n            -depthOfFieldPreview 1\n            -maxConstantTransparency 1\n            -rendererName \"vp2Renderer\" \n            -objectFilterShowInHUD 1\n            -isFiltered 0\n            -colorResolution 256 256 \n            -bumpResolution 512 512 \n"
		+ "            -textureCompression 0\n            -transparencyAlgorithm \"frontAndBackCull\" \n            -transpInShadows 0\n            -cullingOverride \"none\" \n            -lowQualityLighting 0\n            -maximumNumHardwareLights 1\n            -occlusionCulling 0\n            -shadingModel 0\n            -useBaseRenderer 0\n            -useReducedRenderer 0\n            -smallObjectCulling 0\n            -smallObjectThreshold -1 \n            -interactiveDisableShadows 0\n            -interactiveBackFaceCull 0\n            -sortTransparent 1\n            -controllers 1\n            -nurbsCurves 1\n            -nurbsSurfaces 1\n            -polymeshes 1\n            -subdivSurfaces 1\n            -planes 1\n            -lights 1\n            -cameras 1\n            -controlVertices 1\n            -hulls 1\n            -grid 1\n            -imagePlane 1\n            -joints 1\n            -ikHandles 1\n            -deformers 1\n            -dynamics 1\n            -particleInstancers 1\n            -fluids 1\n            -hairSystems 1\n            -follicles 1\n"
		+ "            -nCloths 1\n            -nParticles 1\n            -nRigids 1\n            -dynamicConstraints 1\n            -locators 1\n            -manipulators 1\n            -pluginShapes 1\n            -dimensions 1\n            -handles 1\n            -pivots 1\n            -textures 1\n            -strokes 1\n            -motionTrails 1\n            -clipGhosts 1\n            -greasePencils 1\n            -shadows 0\n            -captureSequenceNumber -1\n            -width 883\n            -height 669\n            -sceneRenderFilter 0\n            $editorName;\n        modelEditor -e -viewSelected 0 $editorName;\n        modelEditor -e \n            -pluginObjects \"gpuCacheDisplayFilter\" 1 \n            $editorName;\n\t\tif (!$useSceneConfig) {\n\t\t\tpanel -e -l $label $panelName;\n\t\t}\n\t}\n\n\n\t$panelName = `sceneUIReplacement -getNextPanel \"outlinerPanel\" (localizedPanelLabel(\"ToggledOutliner\")) `;\n\tif (\"\" != $panelName) {\n\t\t$label = `panel -q -label $panelName`;\n\t\toutlinerPanel -edit -l (localizedPanelLabel(\"ToggledOutliner\")) -mbv $menusOkayInPanels  $panelName;\n"
		+ "\t\t$editorName = $panelName;\n        outlinerEditor -e \n            -docTag \"isolOutln_fromSeln\" \n            -showShapes 0\n            -showAssignedMaterials 0\n            -showTimeEditor 1\n            -showReferenceNodes 1\n            -showReferenceMembers 1\n            -showAttributes 0\n            -showConnected 0\n            -showAnimCurvesOnly 0\n            -showMuteInfo 0\n            -organizeByLayer 1\n            -organizeByClip 1\n            -showAnimLayerWeight 1\n            -autoExpandLayers 1\n            -autoExpand 0\n            -showDagOnly 1\n            -showAssets 1\n            -showContainedOnly 1\n            -showPublishedAsConnected 0\n            -showParentContainers 0\n            -showContainerContents 1\n            -ignoreDagHierarchy 0\n            -expandConnections 0\n            -showUpstreamCurves 1\n            -showUnitlessCurves 1\n            -showCompounds 1\n            -showLeafs 1\n            -showNumericAttrsOnly 0\n            -highlightActive 1\n            -autoSelectNewObjects 0\n"
		+ "            -doNotSelectNewObjects 0\n            -dropIsParent 1\n            -transmitFilters 0\n            -setFilter \"defaultSetFilter\" \n            -showSetMembers 1\n            -allowMultiSelection 1\n            -alwaysToggleSelect 0\n            -directSelect 0\n            -isSet 0\n            -isSetMember 0\n            -displayMode \"DAG\" \n            -expandObjects 0\n            -setsIgnoreFilters 1\n            -containersIgnoreFilters 0\n            -editAttrName 0\n            -showAttrValues 0\n            -highlightSecondary 0\n            -showUVAttrsOnly 0\n            -showTextureNodesOnly 0\n            -attrAlphaOrder \"default\" \n            -animLayerFilterOptions \"allAffecting\" \n            -sortOrder \"none\" \n            -longNames 0\n            -niceNames 1\n            -selectCommand \"<function selCom at 0x7f29c5c04aa0>\" \n            -showNamespace 1\n            -showPinIcons 0\n            -mapMotionTrails 0\n            -ignoreHiddenAttribute 0\n            -ignoreOutlinerColor 0\n            -renderFilterVisible 0\n"
		+ "            -renderFilterIndex 0\n            -selectionOrder \"chronological\" \n            -expandAttribute 0\n            $editorName;\n\t\tif (!$useSceneConfig) {\n\t\t\tpanel -e -l $label $panelName;\n\t\t}\n\t}\n\n\n\t$panelName = `sceneUIReplacement -getNextPanel \"outlinerPanel\" (localizedPanelLabel(\"Outliner\")) `;\n\tif (\"\" != $panelName) {\n\t\t$label = `panel -q -label $panelName`;\n\t\toutlinerPanel -edit -l (localizedPanelLabel(\"Outliner\")) -mbv $menusOkayInPanels  $panelName;\n\t\t$editorName = $panelName;\n        outlinerEditor -e \n            -showShapes 0\n            -showAssignedMaterials 0\n            -showTimeEditor 1\n            -showReferenceNodes 0\n            -showReferenceMembers 0\n            -showAttributes 0\n            -showConnected 0\n            -showAnimCurvesOnly 0\n            -showMuteInfo 0\n            -organizeByLayer 1\n            -organizeByClip 1\n            -showAnimLayerWeight 1\n            -autoExpandLayers 1\n            -autoExpand 0\n            -showDagOnly 1\n            -showAssets 1\n            -showContainedOnly 1\n"
		+ "            -showPublishedAsConnected 0\n            -showParentContainers 0\n            -showContainerContents 1\n            -ignoreDagHierarchy 0\n            -expandConnections 0\n            -showUpstreamCurves 1\n            -showUnitlessCurves 1\n            -showCompounds 1\n            -showLeafs 1\n            -showNumericAttrsOnly 0\n            -highlightActive 1\n            -autoSelectNewObjects 0\n            -doNotSelectNewObjects 0\n            -dropIsParent 1\n            -transmitFilters 0\n            -setFilter \"0\" \n            -showSetMembers 1\n            -allowMultiSelection 1\n            -alwaysToggleSelect 0\n            -directSelect 0\n            -displayMode \"DAG\" \n            -expandObjects 0\n            -setsIgnoreFilters 1\n            -containersIgnoreFilters 0\n            -editAttrName 0\n            -showAttrValues 0\n            -highlightSecondary 0\n            -showUVAttrsOnly 0\n            -showTextureNodesOnly 0\n            -attrAlphaOrder \"default\" \n            -animLayerFilterOptions \"allAffecting\" \n"
		+ "            -sortOrder \"none\" \n            -longNames 0\n            -niceNames 1\n            -showNamespace 1\n            -showPinIcons 0\n            -mapMotionTrails 0\n            -ignoreHiddenAttribute 0\n            -ignoreOutlinerColor 0\n            -renderFilterVisible 0\n            $editorName;\n\t\tif (!$useSceneConfig) {\n\t\t\tpanel -e -l $label $panelName;\n\t\t}\n\t}\n\n\n\t$panelName = `sceneUIReplacement -getNextScriptedPanel \"graphEditor\" (localizedPanelLabel(\"Graph Editor\")) `;\n\tif (\"\" != $panelName) {\n\t\t$label = `panel -q -label $panelName`;\n\t\tscriptedPanel -edit -l (localizedPanelLabel(\"Graph Editor\")) -mbv $menusOkayInPanels  $panelName;\n\n\t\t\t$editorName = ($panelName+\"OutlineEd\");\n            outlinerEditor -e \n                -showShapes 1\n                -showAssignedMaterials 0\n                -showTimeEditor 1\n                -showReferenceNodes 0\n                -showReferenceMembers 0\n                -showAttributes 1\n                -showConnected 1\n                -showAnimCurvesOnly 1\n                -showMuteInfo 0\n"
		+ "                -organizeByLayer 1\n                -organizeByClip 1\n                -showAnimLayerWeight 1\n                -autoExpandLayers 1\n                -autoExpand 1\n                -showDagOnly 0\n                -showAssets 1\n                -showContainedOnly 0\n                -showPublishedAsConnected 0\n                -showParentContainers 0\n                -showContainerContents 0\n                -ignoreDagHierarchy 0\n                -expandConnections 1\n                -showUpstreamCurves 1\n                -showUnitlessCurves 1\n                -showCompounds 0\n                -showLeafs 1\n                -showNumericAttrsOnly 1\n                -highlightActive 0\n                -autoSelectNewObjects 1\n                -doNotSelectNewObjects 0\n                -dropIsParent 1\n                -transmitFilters 1\n                -setFilter \"0\" \n                -showSetMembers 0\n                -allowMultiSelection 1\n                -alwaysToggleSelect 0\n                -directSelect 0\n                -displayMode \"DAG\" \n"
		+ "                -expandObjects 0\n                -setsIgnoreFilters 1\n                -containersIgnoreFilters 0\n                -editAttrName 0\n                -showAttrValues 0\n                -highlightSecondary 0\n                -showUVAttrsOnly 0\n                -showTextureNodesOnly 0\n                -attrAlphaOrder \"default\" \n                -animLayerFilterOptions \"allAffecting\" \n                -sortOrder \"none\" \n                -longNames 0\n                -niceNames 1\n                -showNamespace 1\n                -showPinIcons 1\n                -mapMotionTrails 1\n                -ignoreHiddenAttribute 0\n                -ignoreOutlinerColor 0\n                -renderFilterVisible 0\n                $editorName;\n\n\t\t\t$editorName = ($panelName+\"GraphEd\");\n            animCurveEditor -e \n                -displayValues 0\n                -snapTime \"integer\" \n                -snapValue \"none\" \n                -showPlayRangeShades \"on\" \n                -lockPlayRangeShades \"off\" \n                -smoothness \"fine\" \n"
		+ "                -resultSamples 1\n                -resultScreenSamples 0\n                -resultUpdate \"delayed\" \n                -showUpstreamCurves 1\n                -stackedCurvesMin -1\n                -stackedCurvesMax 1\n                -stackedCurvesSpace 0.2\n                -preSelectionHighlight 0\n                -constrainDrag 0\n                -valueLinesToggle 1\n                -highlightAffectedCurves 0\n                $editorName;\n\t\tif (!$useSceneConfig) {\n\t\t\tpanel -e -l $label $panelName;\n\t\t}\n\t}\n\n\n\t$panelName = `sceneUIReplacement -getNextScriptedPanel \"dopeSheetPanel\" (localizedPanelLabel(\"Dope Sheet\")) `;\n\tif (\"\" != $panelName) {\n\t\t$label = `panel -q -label $panelName`;\n\t\tscriptedPanel -edit -l (localizedPanelLabel(\"Dope Sheet\")) -mbv $menusOkayInPanels  $panelName;\n\n\t\t\t$editorName = ($panelName+\"OutlineEd\");\n            outlinerEditor -e \n                -showShapes 1\n                -showAssignedMaterials 0\n                -showTimeEditor 1\n                -showReferenceNodes 0\n                -showReferenceMembers 0\n"
		+ "                -showAttributes 1\n                -showConnected 1\n                -showAnimCurvesOnly 1\n                -showMuteInfo 0\n                -organizeByLayer 1\n                -organizeByClip 1\n                -showAnimLayerWeight 1\n                -autoExpandLayers 1\n                -autoExpand 0\n                -showDagOnly 0\n                -showAssets 1\n                -showContainedOnly 0\n                -showPublishedAsConnected 0\n                -showParentContainers 0\n                -showContainerContents 0\n                -ignoreDagHierarchy 0\n                -expandConnections 1\n                -showUpstreamCurves 1\n                -showUnitlessCurves 0\n                -showCompounds 1\n                -showLeafs 1\n                -showNumericAttrsOnly 1\n                -highlightActive 0\n                -autoSelectNewObjects 0\n                -doNotSelectNewObjects 1\n                -dropIsParent 1\n                -transmitFilters 0\n                -setFilter \"0\" \n                -showSetMembers 0\n"
		+ "                -allowMultiSelection 1\n                -alwaysToggleSelect 0\n                -directSelect 0\n                -displayMode \"DAG\" \n                -expandObjects 0\n                -setsIgnoreFilters 1\n                -containersIgnoreFilters 0\n                -editAttrName 0\n                -showAttrValues 0\n                -highlightSecondary 0\n                -showUVAttrsOnly 0\n                -showTextureNodesOnly 0\n                -attrAlphaOrder \"default\" \n                -animLayerFilterOptions \"allAffecting\" \n                -sortOrder \"none\" \n                -longNames 0\n                -niceNames 1\n                -showNamespace 1\n                -showPinIcons 0\n                -mapMotionTrails 1\n                -ignoreHiddenAttribute 0\n                -ignoreOutlinerColor 0\n                -renderFilterVisible 0\n                $editorName;\n\n\t\t\t$editorName = ($panelName+\"DopeSheetEd\");\n            dopeSheetEditor -e \n                -displayValues 0\n                -snapTime \"integer\" \n"
		+ "                -snapValue \"none\" \n                -outliner \"dopeSheetPanel1OutlineEd\" \n                -showSummary 1\n                -showScene 0\n                -hierarchyBelow 0\n                -showTicks 1\n                -selectionWindow 0 0 0 0 \n                $editorName;\n\t\tif (!$useSceneConfig) {\n\t\t\tpanel -e -l $label $panelName;\n\t\t}\n\t}\n\n\n\t$panelName = `sceneUIReplacement -getNextScriptedPanel \"timeEditorPanel\" (localizedPanelLabel(\"Time Editor\")) `;\n\tif (\"\" != $panelName) {\n\t\t$label = `panel -q -label $panelName`;\n\t\tscriptedPanel -edit -l (localizedPanelLabel(\"Time Editor\")) -mbv $menusOkayInPanels  $panelName;\n\t\tif (!$useSceneConfig) {\n\t\t\tpanel -e -l $label $panelName;\n\t\t}\n\t}\n\n\n\t$panelName = `sceneUIReplacement -getNextScriptedPanel \"clipEditorPanel\" (localizedPanelLabel(\"Trax Editor\")) `;\n\tif (\"\" != $panelName) {\n\t\t$label = `panel -q -label $panelName`;\n\t\tscriptedPanel -edit -l (localizedPanelLabel(\"Trax Editor\")) -mbv $menusOkayInPanels  $panelName;\n\n\t\t\t$editorName = clipEditorNameFromPanel($panelName);\n"
		+ "            clipEditor -e \n                -displayValues 0\n                -snapTime \"none\" \n                -snapValue \"none\" \n                -initialized 0\n                -manageSequencer 0 \n                $editorName;\n\t\tif (!$useSceneConfig) {\n\t\t\tpanel -e -l $label $panelName;\n\t\t}\n\t}\n\n\n\t$panelName = `sceneUIReplacement -getNextScriptedPanel \"sequenceEditorPanel\" (localizedPanelLabel(\"Camera Sequencer\")) `;\n\tif (\"\" != $panelName) {\n\t\t$label = `panel -q -label $panelName`;\n\t\tscriptedPanel -edit -l (localizedPanelLabel(\"Camera Sequencer\")) -mbv $menusOkayInPanels  $panelName;\n\n\t\t\t$editorName = sequenceEditorNameFromPanel($panelName);\n            clipEditor -e \n                -displayValues 0\n                -snapTime \"none\" \n                -snapValue \"none\" \n                -initialized 0\n                -manageSequencer 1 \n                $editorName;\n\t\tif (!$useSceneConfig) {\n\t\t\tpanel -e -l $label $panelName;\n\t\t}\n\t}\n\n\n\t$panelName = `sceneUIReplacement -getNextScriptedPanel \"hyperGraphPanel\" (localizedPanelLabel(\"Hypergraph Hierarchy\")) `;\n"
		+ "\tif (\"\" != $panelName) {\n\t\t$label = `panel -q -label $panelName`;\n\t\tscriptedPanel -edit -l (localizedPanelLabel(\"Hypergraph Hierarchy\")) -mbv $menusOkayInPanels  $panelName;\n\n\t\t\t$editorName = ($panelName+\"HyperGraphEd\");\n            hyperGraph -e \n                -graphLayoutStyle \"hierarchicalLayout\" \n                -orientation \"horiz\" \n                -mergeConnections 0\n                -zoom 1\n                -animateTransition 0\n                -showRelationships 1\n                -showShapes 0\n                -showDeformers 0\n                -showExpressions 0\n                -showConstraints 0\n                -showConnectionFromSelected 0\n                -showConnectionToSelected 0\n                -showConstraintLabels 0\n                -showUnderworld 0\n                -showInvisible 0\n                -transitionFrames 1\n                -opaqueContainers 0\n                -freeform 0\n                -imagePosition 0 0 \n                -imageScale 1\n                -imageEnabled 0\n                -graphType \"DAG\" \n"
		+ "                -heatMapDisplay 0\n                -updateSelection 1\n                -updateNodeAdded 1\n                -useDrawOverrideColor 0\n                -limitGraphTraversal -1\n                -range 0 0 \n                -iconSize \"smallIcons\" \n                -showCachedConnections 0\n                $editorName;\n\t\tif (!$useSceneConfig) {\n\t\t\tpanel -e -l $label $panelName;\n\t\t}\n\t}\n\n\n\t$panelName = `sceneUIReplacement -getNextScriptedPanel \"hyperShadePanel\" (localizedPanelLabel(\"Hypershade\")) `;\n\tif (\"\" != $panelName) {\n\t\t$label = `panel -q -label $panelName`;\n\t\tscriptedPanel -edit -l (localizedPanelLabel(\"Hypershade\")) -mbv $menusOkayInPanels  $panelName;\n\t\tif (!$useSceneConfig) {\n\t\t\tpanel -e -l $label $panelName;\n\t\t}\n\t}\n\n\n\t$panelName = `sceneUIReplacement -getNextScriptedPanel \"visorPanel\" (localizedPanelLabel(\"Visor\")) `;\n\tif (\"\" != $panelName) {\n\t\t$label = `panel -q -label $panelName`;\n\t\tscriptedPanel -edit -l (localizedPanelLabel(\"Visor\")) -mbv $menusOkayInPanels  $panelName;\n\t\tif (!$useSceneConfig) {\n"
		+ "\t\t\tpanel -e -l $label $panelName;\n\t\t}\n\t}\n\n\n\t$panelName = `sceneUIReplacement -getNextScriptedPanel \"nodeEditorPanel\" (localizedPanelLabel(\"Node Editor\")) `;\n\tif ($nodeEditorPanelVisible || $nodeEditorWorkspaceControlOpen) {\n\t\tif (\"\" == $panelName) {\n\t\t\tif ($useSceneConfig) {\n\t\t\t\t$panelName = `scriptedPanel -unParent  -type \"nodeEditorPanel\" -l (localizedPanelLabel(\"Node Editor\")) -mbv $menusOkayInPanels `;\n\n\t\t\t$editorName = ($panelName+\"NodeEditorEd\");\n            nodeEditor -e \n                -allAttributes 0\n                -allNodes 0\n                -autoSizeNodes 1\n                -consistentNameSize 1\n                -createNodeCommand \"nodeEdCreateNodeCommand\" \n                -connectNodeOnCreation 0\n                -connectOnDrop 0\n                -copyConnectionsOnPaste 0\n                -connectionStyle \"bezier\" \n                -defaultPinnedState 0\n                -additiveGraphingMode 0\n                -settingsChangedCallback \"nodeEdSyncControls\" \n                -traversalDepthLimit -1\n                -keyPressCommand \"nodeEdKeyPressCommand\" \n"
		+ "                -nodeTitleMode \"name\" \n                -gridSnap 0\n                -gridVisibility 1\n                -crosshairOnEdgeDragging 0\n                -popupMenuScript \"nodeEdBuildPanelMenus\" \n                -showNamespace 1\n                -showShapes 1\n                -showSGShapes 0\n                -showTransforms 1\n                -useAssets 1\n                -syncedSelection 1\n                -extendToShapes 1\n                -editorMode \"default\" \n                -hasWatchpoint 0\n                $editorName;\n\t\t\t}\n\t\t} else {\n\t\t\t$label = `panel -q -label $panelName`;\n\t\t\tscriptedPanel -edit -l (localizedPanelLabel(\"Node Editor\")) -mbv $menusOkayInPanels  $panelName;\n\n\t\t\t$editorName = ($panelName+\"NodeEditorEd\");\n            nodeEditor -e \n                -allAttributes 0\n                -allNodes 0\n                -autoSizeNodes 1\n                -consistentNameSize 1\n                -createNodeCommand \"nodeEdCreateNodeCommand\" \n                -connectNodeOnCreation 0\n                -connectOnDrop 0\n"
		+ "                -copyConnectionsOnPaste 0\n                -connectionStyle \"bezier\" \n                -defaultPinnedState 0\n                -additiveGraphingMode 0\n                -settingsChangedCallback \"nodeEdSyncControls\" \n                -traversalDepthLimit -1\n                -keyPressCommand \"nodeEdKeyPressCommand\" \n                -nodeTitleMode \"name\" \n                -gridSnap 0\n                -gridVisibility 1\n                -crosshairOnEdgeDragging 0\n                -popupMenuScript \"nodeEdBuildPanelMenus\" \n                -showNamespace 1\n                -showShapes 1\n                -showSGShapes 0\n                -showTransforms 1\n                -useAssets 1\n                -syncedSelection 1\n                -extendToShapes 1\n                -editorMode \"default\" \n                -hasWatchpoint 0\n                $editorName;\n\t\t\tif (!$useSceneConfig) {\n\t\t\t\tpanel -e -l $label $panelName;\n\t\t\t}\n\t\t}\n\t}\n\n\n\t$panelName = `sceneUIReplacement -getNextScriptedPanel \"createNodePanel\" (localizedPanelLabel(\"Create Node\")) `;\n"
		+ "\tif (\"\" != $panelName) {\n\t\t$label = `panel -q -label $panelName`;\n\t\tscriptedPanel -edit -l (localizedPanelLabel(\"Create Node\")) -mbv $menusOkayInPanels  $panelName;\n\t\tif (!$useSceneConfig) {\n\t\t\tpanel -e -l $label $panelName;\n\t\t}\n\t}\n\n\n\t$panelName = `sceneUIReplacement -getNextScriptedPanel \"polyTexturePlacementPanel\" (localizedPanelLabel(\"UV Editor\")) `;\n\tif (\"\" != $panelName) {\n\t\t$label = `panel -q -label $panelName`;\n\t\tscriptedPanel -edit -l (localizedPanelLabel(\"UV Editor\")) -mbv $menusOkayInPanels  $panelName;\n\t\tif (!$useSceneConfig) {\n\t\t\tpanel -e -l $label $panelName;\n\t\t}\n\t}\n\n\n\t$panelName = `sceneUIReplacement -getNextScriptedPanel \"renderWindowPanel\" (localizedPanelLabel(\"Render View\")) `;\n\tif (\"\" != $panelName) {\n\t\t$label = `panel -q -label $panelName`;\n\t\tscriptedPanel -edit -l (localizedPanelLabel(\"Render View\")) -mbv $menusOkayInPanels  $panelName;\n\t\tif (!$useSceneConfig) {\n\t\t\tpanel -e -l $label $panelName;\n\t\t}\n\t}\n\n\n\t$panelName = `sceneUIReplacement -getNextPanel \"shapePanel\" (localizedPanelLabel(\"Shape Editor\")) `;\n"
		+ "\tif (\"\" != $panelName) {\n\t\t$label = `panel -q -label $panelName`;\n\t\tshapePanel -edit -l (localizedPanelLabel(\"Shape Editor\")) -mbv $menusOkayInPanels  $panelName;\n\t\tif (!$useSceneConfig) {\n\t\t\tpanel -e -l $label $panelName;\n\t\t}\n\t}\n\n\n\t$panelName = `sceneUIReplacement -getNextPanel \"posePanel\" (localizedPanelLabel(\"Pose Editor\")) `;\n\tif (\"\" != $panelName) {\n\t\t$label = `panel -q -label $panelName`;\n\t\tposePanel -edit -l (localizedPanelLabel(\"Pose Editor\")) -mbv $menusOkayInPanels  $panelName;\n\t\tif (!$useSceneConfig) {\n\t\t\tpanel -e -l $label $panelName;\n\t\t}\n\t}\n\n\n\t$panelName = `sceneUIReplacement -getNextScriptedPanel \"dynRelEdPanel\" (localizedPanelLabel(\"Dynamic Relationships\")) `;\n\tif (\"\" != $panelName) {\n\t\t$label = `panel -q -label $panelName`;\n\t\tscriptedPanel -edit -l (localizedPanelLabel(\"Dynamic Relationships\")) -mbv $menusOkayInPanels  $panelName;\n\t\tif (!$useSceneConfig) {\n\t\t\tpanel -e -l $label $panelName;\n\t\t}\n\t}\n\n\n\t$panelName = `sceneUIReplacement -getNextScriptedPanel \"relationshipPanel\" (localizedPanelLabel(\"Relationship Editor\")) `;\n"
		+ "\tif (\"\" != $panelName) {\n\t\t$label = `panel -q -label $panelName`;\n\t\tscriptedPanel -edit -l (localizedPanelLabel(\"Relationship Editor\")) -mbv $menusOkayInPanels  $panelName;\n\t\tif (!$useSceneConfig) {\n\t\t\tpanel -e -l $label $panelName;\n\t\t}\n\t}\n\n\n\t$panelName = `sceneUIReplacement -getNextScriptedPanel \"referenceEditorPanel\" (localizedPanelLabel(\"Reference Editor\")) `;\n\tif (\"\" != $panelName) {\n\t\t$label = `panel -q -label $panelName`;\n\t\tscriptedPanel -edit -l (localizedPanelLabel(\"Reference Editor\")) -mbv $menusOkayInPanels  $panelName;\n\t\tif (!$useSceneConfig) {\n\t\t\tpanel -e -l $label $panelName;\n\t\t}\n\t}\n\n\n\t$panelName = `sceneUIReplacement -getNextScriptedPanel \"componentEditorPanel\" (localizedPanelLabel(\"Component Editor\")) `;\n\tif (\"\" != $panelName) {\n\t\t$label = `panel -q -label $panelName`;\n\t\tscriptedPanel -edit -l (localizedPanelLabel(\"Component Editor\")) -mbv $menusOkayInPanels  $panelName;\n\t\tif (!$useSceneConfig) {\n\t\t\tpanel -e -l $label $panelName;\n\t\t}\n\t}\n\n\n\t$panelName = `sceneUIReplacement -getNextScriptedPanel \"dynPaintScriptedPanelType\" (localizedPanelLabel(\"Paint Effects\")) `;\n"
		+ "\tif (\"\" != $panelName) {\n\t\t$label = `panel -q -label $panelName`;\n\t\tscriptedPanel -edit -l (localizedPanelLabel(\"Paint Effects\")) -mbv $menusOkayInPanels  $panelName;\n\t\tif (!$useSceneConfig) {\n\t\t\tpanel -e -l $label $panelName;\n\t\t}\n\t}\n\n\n\t$panelName = `sceneUIReplacement -getNextScriptedPanel \"scriptEditorPanel\" (localizedPanelLabel(\"Script Editor\")) `;\n\tif (\"\" != $panelName) {\n\t\t$label = `panel -q -label $panelName`;\n\t\tscriptedPanel -edit -l (localizedPanelLabel(\"Script Editor\")) -mbv $menusOkayInPanels  $panelName;\n\t\tif (!$useSceneConfig) {\n\t\t\tpanel -e -l $label $panelName;\n\t\t}\n\t}\n\n\n\t$panelName = `sceneUIReplacement -getNextScriptedPanel \"profilerPanel\" (localizedPanelLabel(\"Profiler Tool\")) `;\n\tif (\"\" != $panelName) {\n\t\t$label = `panel -q -label $panelName`;\n\t\tscriptedPanel -edit -l (localizedPanelLabel(\"Profiler Tool\")) -mbv $menusOkayInPanels  $panelName;\n\t\tif (!$useSceneConfig) {\n\t\t\tpanel -e -l $label $panelName;\n\t\t}\n\t}\n\n\n\t$panelName = `sceneUIReplacement -getNextScriptedPanel \"contentBrowserPanel\" (localizedPanelLabel(\"Content Browser\")) `;\n"
		+ "\tif (\"\" != $panelName) {\n\t\t$label = `panel -q -label $panelName`;\n\t\tscriptedPanel -edit -l (localizedPanelLabel(\"Content Browser\")) -mbv $menusOkayInPanels  $panelName;\n\t\tif (!$useSceneConfig) {\n\t\t\tpanel -e -l $label $panelName;\n\t\t}\n\t}\n\n\n\tif ($useSceneConfig) {\n        string $configName = `getPanel -cwl (localizedPanelLabel(\"Current Layout\"))`;\n        if (\"\" != $configName) {\n\t\t\tpanelConfiguration -edit -label (localizedPanelLabel(\"Current Layout\")) \n\t\t\t\t-userCreated false\n\t\t\t\t-defaultImage \"vacantCell.xP:/\"\n\t\t\t\t-image \"\"\n\t\t\t\t-sc false\n\t\t\t\t-configString \"global string $gMainPane; paneLayout -e -cn \\\"single\\\" -ps 1 100 100 $gMainPane;\"\n\t\t\t\t-removeAllPanels\n\t\t\t\t-ap false\n\t\t\t\t\t(localizedPanelLabel(\"Persp View\")) \n\t\t\t\t\t\"modelPanel\"\n"
		+ "\t\t\t\t\t\"$panelName = `modelPanel -unParent -l (localizedPanelLabel(\\\"Persp View\\\")) -mbv $menusOkayInPanels `;\\n$editorName = $panelName;\\nmodelEditor -e \\n    -cam `findStartUpCamera persp` \\n    -useInteractiveMode 0\\n    -displayLights \\\"default\\\" \\n    -displayAppearance \\\"smoothShaded\\\" \\n    -activeOnly 0\\n    -ignorePanZoom 0\\n    -wireframeOnShaded 0\\n    -headsUpDisplay 1\\n    -holdOuts 1\\n    -selectionHiliteDisplay 1\\n    -useDefaultMaterial 0\\n    -bufferMode \\\"double\\\" \\n    -twoSidedLighting 0\\n    -backfaceCulling 0\\n    -xray 0\\n    -jointXray 0\\n    -activeComponentsXray 0\\n    -displayTextures 0\\n    -smoothWireframe 0\\n    -lineWidth 1\\n    -textureAnisotropic 0\\n    -textureHilight 1\\n    -textureSampling 2\\n    -textureDisplay \\\"modulate\\\" \\n    -textureMaxSize 32768\\n    -fogging 0\\n    -fogSource \\\"fragment\\\" \\n    -fogMode \\\"linear\\\" \\n    -fogStart 0\\n    -fogEnd 100\\n    -fogDensity 0.1\\n    -fogColor 0.5 0.5 0.5 1 \\n    -depthOfFieldPreview 1\\n    -maxConstantTransparency 1\\n    -rendererName \\\"vp2Renderer\\\" \\n    -objectFilterShowInHUD 1\\n    -isFiltered 0\\n    -colorResolution 256 256 \\n    -bumpResolution 512 512 \\n    -textureCompression 0\\n    -transparencyAlgorithm \\\"frontAndBackCull\\\" \\n    -transpInShadows 0\\n    -cullingOverride \\\"none\\\" \\n    -lowQualityLighting 0\\n    -maximumNumHardwareLights 1\\n    -occlusionCulling 0\\n    -shadingModel 0\\n    -useBaseRenderer 0\\n    -useReducedRenderer 0\\n    -smallObjectCulling 0\\n    -smallObjectThreshold -1 \\n    -interactiveDisableShadows 0\\n    -interactiveBackFaceCull 0\\n    -sortTransparent 1\\n    -controllers 1\\n    -nurbsCurves 1\\n    -nurbsSurfaces 1\\n    -polymeshes 1\\n    -subdivSurfaces 1\\n    -planes 1\\n    -lights 1\\n    -cameras 1\\n    -controlVertices 1\\n    -hulls 1\\n    -grid 1\\n    -imagePlane 1\\n    -joints 1\\n    -ikHandles 1\\n    -deformers 1\\n    -dynamics 1\\n    -particleInstancers 1\\n    -fluids 1\\n    -hairSystems 1\\n    -follicles 1\\n    -nCloths 1\\n    -nParticles 1\\n    -nRigids 1\\n    -dynamicConstraints 1\\n    -locators 1\\n    -manipulators 1\\n    -pluginShapes 1\\n    -dimensions 1\\n    -handles 1\\n    -pivots 1\\n    -textures 1\\n    -strokes 1\\n    -motionTrails 1\\n    -clipGhosts 1\\n    -greasePencils 1\\n    -shadows 0\\n    -captureSequenceNumber -1\\n    -width 883\\n    -height 669\\n    -sceneRenderFilter 0\\n    $editorName;\\nmodelEditor -e -viewSelected 0 $editorName;\\nmodelEditor -e \\n    -pluginObjects \\\"gpuCacheDisplayFilter\\\" 1 \\n    $editorName\"\n"
		+ "\t\t\t\t\t\"modelPanel -edit -l (localizedPanelLabel(\\\"Persp View\\\")) -mbv $menusOkayInPanels  $panelName;\\n$editorName = $panelName;\\nmodelEditor -e \\n    -cam `findStartUpCamera persp` \\n    -useInteractiveMode 0\\n    -displayLights \\\"default\\\" \\n    -displayAppearance \\\"smoothShaded\\\" \\n    -activeOnly 0\\n    -ignorePanZoom 0\\n    -wireframeOnShaded 0\\n    -headsUpDisplay 1\\n    -holdOuts 1\\n    -selectionHiliteDisplay 1\\n    -useDefaultMaterial 0\\n    -bufferMode \\\"double\\\" \\n    -twoSidedLighting 0\\n    -backfaceCulling 0\\n    -xray 0\\n    -jointXray 0\\n    -activeComponentsXray 0\\n    -displayTextures 0\\n    -smoothWireframe 0\\n    -lineWidth 1\\n    -textureAnisotropic 0\\n    -textureHilight 1\\n    -textureSampling 2\\n    -textureDisplay \\\"modulate\\\" \\n    -textureMaxSize 32768\\n    -fogging 0\\n    -fogSource \\\"fragment\\\" \\n    -fogMode \\\"linear\\\" \\n    -fogStart 0\\n    -fogEnd 100\\n    -fogDensity 0.1\\n    -fogColor 0.5 0.5 0.5 1 \\n    -depthOfFieldPreview 1\\n    -maxConstantTransparency 1\\n    -rendererName \\\"vp2Renderer\\\" \\n    -objectFilterShowInHUD 1\\n    -isFiltered 0\\n    -colorResolution 256 256 \\n    -bumpResolution 512 512 \\n    -textureCompression 0\\n    -transparencyAlgorithm \\\"frontAndBackCull\\\" \\n    -transpInShadows 0\\n    -cullingOverride \\\"none\\\" \\n    -lowQualityLighting 0\\n    -maximumNumHardwareLights 1\\n    -occlusionCulling 0\\n    -shadingModel 0\\n    -useBaseRenderer 0\\n    -useReducedRenderer 0\\n    -smallObjectCulling 0\\n    -smallObjectThreshold -1 \\n    -interactiveDisableShadows 0\\n    -interactiveBackFaceCull 0\\n    -sortTransparent 1\\n    -controllers 1\\n    -nurbsCurves 1\\n    -nurbsSurfaces 1\\n    -polymeshes 1\\n    -subdivSurfaces 1\\n    -planes 1\\n    -lights 1\\n    -cameras 1\\n    -controlVertices 1\\n    -hulls 1\\n    -grid 1\\n    -imagePlane 1\\n    -joints 1\\n    -ikHandles 1\\n    -deformers 1\\n    -dynamics 1\\n    -particleInstancers 1\\n    -fluids 1\\n    -hairSystems 1\\n    -follicles 1\\n    -nCloths 1\\n    -nParticles 1\\n    -nRigids 1\\n    -dynamicConstraints 1\\n    -locators 1\\n    -manipulators 1\\n    -pluginShapes 1\\n    -dimensions 1\\n    -handles 1\\n    -pivots 1\\n    -textures 1\\n    -strokes 1\\n    -motionTrails 1\\n    -clipGhosts 1\\n    -greasePencils 1\\n    -shadows 0\\n    -captureSequenceNumber -1\\n    -width 883\\n    -height 669\\n    -sceneRenderFilter 0\\n    $editorName;\\nmodelEditor -e -viewSelected 0 $editorName;\\nmodelEditor -e \\n    -pluginObjects \\\"gpuCacheDisplayFilter\\\" 1 \\n    $editorName\"\n"
		+ "\t\t\t\t$configName;\n\n            setNamedPanelLayout (localizedPanelLabel(\"Current Layout\"));\n        }\n\n        panelHistory -e -clear mainPanelHistory;\n        sceneUIReplacement -clear;\n\t}\n\n\ngrid -spacing 5 -size 12 -divisions 5 -displayAxes yes -displayGridLines yes -displayDivisionLines yes -displayPerspectiveLabels no -displayOrthographicLabels no -displayAxesBold yes -perspectiveLabelPosition axis -orthographicLabelPosition edge;\nviewManip -drawCompass 0 -compassAngle 0 -frontParameters \"\" -homeParameters \"\" -selectionLockParameters \"\";\n}\n");
	setAttr ".st" 3;
createNode script -n "sceneConfigurationScriptNode";
	rename -uid "0994A49C-4860-B36A-F721-9EB4EDC309DE";
	setAttr ".b" -type "string" "playbackOptions -min 1 -max 120 -ast 1 -aet 200 ";
	setAttr ".st" 6;
createNode polyTweak -n "polyTweak4";
	rename -uid "AF96BD89-48C6-0509-55D5-0ABFC489A24F";
	setAttr ".uopa" yes;
	setAttr -s 10 ".tk[0:9]" -type "float3"  -1.4901161e-08 5.9604645e-08
		 1.36881733 -5.9604645e-08 -1.4901161e-08 1.36881733 5.9604645e-08 1.4901161e-08 1.36881733
		 1.4901161e-08 -5.9604645e-08 1.36881733 0 0 -1.36881745 0 0 -1.36881745 0 0 -1.36881745
		 0 0 -1.36881745 0 0 1.29511237 0 0 -1.29511237;
createNode transformGeometry -n "transformGeometry1";
	rename -uid "073D3575-41CD-615D-9DA3-0398B60B86B8";
	setAttr ".txf" -type "matrix" 0.70710678118654746 -0.70710678118654768 0 0 0.70710678118654768 0.70710678118654746 0 0
		 0 0 1 0 0 0 0 1;
createNode polySplit -n "polySplit1";
	rename -uid "8F0E9A53-4636-86B6-80FA-D083200EC1E1";
	setAttr -s 4 ".e[0:3]"  1 0.5 0.5 1;
	setAttr -s 4 ".d[0:3]"  -2147483631 -2147483646 -2147483647 -2147483634;
	setAttr ".sma" 180;
	setAttr ".m2015" yes;
createNode polyCircularize -n "polyCircularize1";
	rename -uid "39BD6A63-40F4-46AF-7430-D0BBC3BFDF88";
	setAttr ".uopa" yes;
	setAttr ".ics" -type "componentList" 3 "e[0:5]" "e[8:9]" "e[20:21]";
	setAttr ".ix" -type "matrix" 1 0 0 0 0 1 0 0 0 0 1 0 0 0 0 1;
	setAttr ".nor" 2;
createNode polyTweak -n "polyTweak5";
	rename -uid "2839B95E-4F88-494A-187F-60A85D3E1503";
	setAttr ".uopa" yes;
	setAttr -s 4 ".tk";
	setAttr ".tk[10]" -type "float3" 0.18154798 0.17377618 0 ;
	setAttr ".tk[11]" -type "float3" 0.18154798 0.17377618 0 ;
createNode polySoftEdge -n "polySoftEdge2";
	rename -uid "2674CD2D-4EBE-D158-7669-B087DFE932FF";
	setAttr ".uopa" yes;
	setAttr ".ics" -type "componentList" 1 "e[*]";
	setAttr ".ix" -type "matrix" 1 0 0 0 0 1 0 0 0 0 1 0 0 0 0 1;
	setAttr ".a" 180;
createNode polyCylinder -n "polyCylinder1";
	rename -uid "598BCC9A-4FF0-9D29-BA10-8CA75FED2E44";
	setAttr ".sa" 8;
	setAttr ".sc" 1;
	setAttr ".cuv" 3;
createNode polySplitRing -n "polySplitRing1";
	rename -uid "D483D26E-4F08-FAD9-8295-67977E892BA4";
	setAttr ".uopa" yes;
	setAttr ".ics" -type "componentList" 1 "e[16:23]";
	setAttr ".ix" -type "matrix" 0.62056144267389024 0 0 0 0 -1.6572899233542062e-15 3.7318851406314404 0
		 0 -0.62056144267389024 -2.7558464074046285e-16 0 0 0 0 1;
	setAttr ".wt" 0.48304671049118042;
	setAttr ".re" 16;
	setAttr ".sma" 29.999999999999996;
	setAttr ".stp" 2;
	setAttr ".div" 1;
	setAttr ".p[0]"  0 0 1;
	setAttr ".fq" yes;
createNode polySplitRing -n "polySplitRing2";
	rename -uid "705AD6F1-4BB7-35EA-6E60-839C209C550A";
	setAttr ".uopa" yes;
	setAttr ".ics" -type "componentList" 7 "e[40:41]" "e[43]" "e[45]" "e[47]" "e[49]" "e[51]" "e[53]";
	setAttr ".ix" -type "matrix" 0.62056144267389024 0 0 0 0 -1.6572899233542062e-15 3.7318851406314404 0
		 0 -0.62056144267389024 -2.7558464074046285e-16 0 0 0 0 1;
	setAttr ".wt" 0.25958523154258728;
	setAttr ".re" 40;
	setAttr ".sma" 29.999999999999996;
	setAttr ".stp" 2;
	setAttr ".div" 1;
	setAttr ".p[0]"  0 0 1;
	setAttr ".fq" yes;
createNode polySplitRing -n "polySplitRing3";
	rename -uid "59C5EDC8-4EAF-0397-CF78-5ABCBBD23BF2";
	setAttr ".uopa" yes;
	setAttr ".ics" -type "componentList" 1 "e[16:23]";
	setAttr ".ix" -type "matrix" 0.62056144267389024 0 0 0 0 -1.6572899233542062e-15 3.7318851406314404 0
		 0 -0.62056144267389024 -2.7558464074046285e-16 0 0 0 0 1;
	setAttr ".wt" 0.40372022986412048;
	setAttr ".re" 16;
	setAttr ".sma" 29.999999999999996;
	setAttr ".stp" 2;
	setAttr ".div" 1;
	setAttr ".p[0]"  0 0 1;
	setAttr ".fq" yes;
createNode polySplitRing -n "polySplitRing4";
	rename -uid "A8932073-4C06-A621-06BD-5C9814CCC3F8";
	setAttr ".uopa" yes;
	setAttr ".ics" -type "componentList" 1 "e[16:23]";
	setAttr ".ix" -type "matrix" 0.62056144267389024 0 0 0 0 -1.6572899233542062e-15 3.7318851406314404 0
		 0 -0.62056144267389024 -2.7558464074046285e-16 0 0 0 0 1;
	setAttr ".wt" 0.42215347290039063;
	setAttr ".re" 16;
	setAttr ".sma" 29.999999999999996;
	setAttr ".stp" 2;
	setAttr ".div" 1;
	setAttr ".p[0]"  0 0 1;
	setAttr ".fq" yes;
createNode polyMergeVert -n "polyMergeVert3";
	rename -uid "7BB97D88-433A-67D7-C4E9-EBBD0D5D91A5";
	setAttr ".ics" -type "componentList" 2 "vtx[8:15]" "vtx[17]";
	setAttr ".ix" -type "matrix" 0.62056144267389024 0 0 0 0 -1.6572899233542062e-15 3.7318851406314404 0
		 0 -0.62056144267389024 -2.7558464074046285e-16 0 0 0 0 1;
	setAttr ".d" 1e-06;
createNode polyTweak -n "polyTweak6";
	rename -uid "B0533310-4709-F64A-5A1A-9492191EF9AC";
	setAttr ".uopa" yes;
	setAttr -s 10 ".tk";
	setAttr ".tk[8]" -type "float3" -0.70710671 0 0.70710671 ;
	setAttr ".tk[9]" -type "float3" 1.9868216e-08 0 0.99999988 ;
	setAttr ".tk[10]" -type "float3" 0.70710671 0 0.70710671 ;
	setAttr ".tk[11]" -type "float3" 0.99999988 0 1.3245477e-08 ;
	setAttr ".tk[12]" -type "float3" 0.70710671 0 -0.70710671 ;
	setAttr ".tk[13]" -type "float3" 1.9868216e-08 0 -0.99999994 ;
	setAttr ".tk[14]" -type "float3" -0.70710677 0 -0.70710677 ;
	setAttr ".tk[15]" -type "float3" -1 0 1.3245477e-08 ;
	setAttr ".tk[17]" -type "float3" 1.9868216e-08 0 1.3245477e-08 ;
createNode polyExtrudeFace -n "polyExtrudeFace2";
	rename -uid "6B62C37F-49C1-6245-F475-EABBEACA76BE";
	setAttr ".ics" -type "componentList" 1 "f[32:39]";
	setAttr ".ix" -type "matrix" 0.62056144267389024 0 0 0 0 -1.6572899233542062e-15 3.7318851406314404 0
		 0 -0.62056144267389024 -2.7558464074046285e-16 0 0 0 0 1;
	setAttr ".ws" yes;
	setAttr ".pvt" -type "float3" 3.6988343e-08 -1.8494172e-08 -0.9329713 ;
	setAttr ".rs" 59536;
	setAttr ".lt" -type "double3" 2.9490299091605721e-17 2.9490299091605721e-17 0.053748360454818907 ;
	setAttr ".c[0]"  0 1 1;
	setAttr ".cbn" -type "double3" -0.35524556515996508 -0.35524560214830864 -1.8659425703157204 ;
	setAttr ".cbx" -type "double3" 0.3552456391366538 0.35524556515996591 1.5126601334848074e-16 ;
createNode polyTweak -n "polyTweak7";
	rename -uid "8C4075F7-40F3-A9D4-13D9-B99996D5B778";
	setAttr ".uopa" yes;
	setAttr -s 42 ".tk";
	setAttr ".tk[0]" -type "float3" -0.21015528 0.037150592 0.2101552 ;
	setAttr ".tk[1]" -type "float3" 1.7714783e-08 0.037150592 0.2972047 ;
	setAttr ".tk[2]" -type "float3" 0.21015523 0.037150592 0.2101552 ;
	setAttr ".tk[3]" -type "float3" 0.29720473 0.037150592 8.8573913e-09 ;
	setAttr ".tk[4]" -type "float3" 0.21015523 0.037150592 -0.21015514 ;
	setAttr ".tk[5]" -type "float3" 1.7714783e-08 0.037150592 -0.29720473 ;
	setAttr ".tk[6]" -type "float3" -0.21015517 0.037150592 -0.2101552 ;
	setAttr ".tk[7]" -type "float3" -0.29720473 0.037150592 8.8573913e-09 ;
	setAttr ".tk[10]" -type "float3" -0.31898201 0 0.31898209 ;
	setAttr ".tk[11]" -type "float3" -0.45110863 0 1.3444087e-08 ;
	setAttr ".tk[12]" -type "float3" -0.31898206 0 -0.31898209 ;
	setAttr ".tk[13]" -type "float3" 2.6888173e-08 0 -0.4511086 ;
	setAttr ".tk[14]" -type "float3" 0.31898212 0 -0.31898203 ;
	setAttr ".tk[15]" -type "float3" 0.45110863 0 1.3444087e-08 ;
	setAttr ".tk[16]" -type "float3" 0.31898212 0 0.31898209 ;
	setAttr ".tk[17]" -type "float3" 2.6888173e-08 0 0.4511086 ;
	setAttr ".tk[18]" -type "float3" -0.30960712 0 0.30960721 ;
	setAttr ".tk[19]" -type "float3" -0.43785053 0 1.3048963e-08 ;
	setAttr ".tk[20]" -type "float3" -0.30960718 0 -0.30960721 ;
	setAttr ".tk[21]" -type "float3" 2.6097926e-08 0 -0.43785053 ;
	setAttr ".tk[22]" -type "float3" 0.30960724 0 -0.30960715 ;
	setAttr ".tk[23]" -type "float3" 0.43785053 0 1.3048963e-08 ;
	setAttr ".tk[24]" -type "float3" 0.30960724 0 0.30960721 ;
	setAttr ".tk[25]" -type "float3" 2.6097926e-08 0 0.43785053 ;
	setAttr ".tk[26]" -type "float3" -0.30231741 0 0.30231747 ;
	setAttr ".tk[27]" -type "float3" -0.42754155 0 1.2741733e-08 ;
	setAttr ".tk[28]" -type "float3" -0.30231747 0 -0.30231747 ;
	setAttr ".tk[29]" -type "float3" 2.5483466e-08 0 -0.42754155 ;
	setAttr ".tk[30]" -type "float3" 0.3023175 0 -0.30231744 ;
	setAttr ".tk[31]" -type "float3" 0.42754155 0 1.2741733e-08 ;
	setAttr ".tk[32]" -type "float3" 0.3023175 0 0.30231747 ;
	setAttr ".tk[33]" -type "float3" 2.5483466e-08 0 0.42754155 ;
	setAttr ".tk[34]" -type "float3" -0.21015511 -0.037150592 0.2101552 ;
	setAttr ".tk[35]" -type "float3" -0.2972047 -0.037150592 8.8573913e-09 ;
	setAttr ".tk[36]" -type "float3" -0.21015517 -0.037150592 -0.2101552 ;
	setAttr ".tk[37]" -type "float3" 1.7714783e-08 -0.037150592 -0.29720467 ;
	setAttr ".tk[38]" -type "float3" 0.21015523 -0.037150592 -0.21015514 ;
	setAttr ".tk[39]" -type "float3" 0.2972047 -0.037150592 8.8573913e-09 ;
	setAttr ".tk[40]" -type "float3" 0.21015523 -0.037150592 0.2101552 ;
	setAttr ".tk[41]" -type "float3" 1.7714783e-08 -0.037150592 0.29720467 ;
createNode polySoftEdge -n "polySoftEdge3";
	rename -uid "C98068CB-47A8-5A6A-C547-03BD2D2BB993";
	setAttr ".uopa" yes;
	setAttr ".ics" -type "componentList" 1 "e[*]";
	setAttr ".ix" -type "matrix" 0.62056144267389024 0 0 0 0 -1.6572899233542062e-15 3.7318851406314404 0
		 0 -0.62056144267389024 -2.7558464074046285e-16 0 0 0 0 1;
	setAttr ".a" 180;
createNode polyTweak -n "polyTweak8";
	rename -uid "590EDEAF-47EB-173D-EA01-75AC0E1E9543";
	setAttr ".uopa" yes;
	setAttr -s 16 ".tk[42:57]" -type "float3"  0 0.016325179 0 0 0.016325179
		 0 0 -0.016325176 0 0 -0.016325176 0 0 0.016325179 0 0 -0.016325176 0 0 0.016325179
		 0 0 -0.016325176 0 0 0.016325179 0 0 -0.016325176 0 0 0.016325179 0 0 -0.016325176
		 0 0 0.016325179 0 0 -0.016325176 0 0 0.016325179 0 0 -0.016325176 0;
createNode polySoftEdge -n "polySoftEdge4";
	rename -uid "7EA14D8F-437B-AC58-B7E2-1580EFB92B58";
	setAttr ".uopa" yes;
	setAttr ".ics" -type "componentList" 23 "e[26]" "e[28]" "e[30]" "e[32]" "e[34]" "e[36]" "e[38:39]" "e[56:63]" "e[82]" "e[86]" "e[89]" "e[91]" "e[94]" "e[96]" "e[99]" "e[101]" "e[104]" "e[106]" "e[109]" "e[111]" "e[114]" "e[116]" "e[118:119]";
	setAttr ".ix" -type "matrix" 0.62056144267389024 0 0 0 0 -1.6572899233542062e-15 3.7318851406314404 0
		 0 -0.62056144267389024 -2.7558464074046285e-16 0 0 0 0 1;
	setAttr ".a" 0;
createNode polyExtrudeFace -n "polyExtrudeFace3";
	rename -uid "84D08D59-4D36-8F2B-5E17-4D9B4490AC0F";
	setAttr ".ics" -type "componentList" 1 "f[0:7]";
	setAttr ".ix" -type "matrix" 0.62056144267389024 0 0 0 0 -1.6572899233542062e-15 3.7318851406314404 0
		 0 -0.62056144267389024 -2.7558464074046285e-16 0 0 0 0 1;
	setAttr ".ws" yes;
	setAttr ".pvt" -type "float3" 5.5482516e-08 -1.8494172e-08 -3.2653995 ;
	setAttr ".rs" 38959;
	setAttr ".lt" -type "double3" 3.5773186827894458e-17 -1.0930329045509368e-16 0.14232642294102313 ;
	setAttr ".ls" -type "double3" 1 0.84333036722806232 1 ;
	setAttr ".c[0]"  0 1 1;
	setAttr ".cbn" -type "double3" -0.43075016839221492 -0.4307502423689023 -3.5892009285038728 ;
	setAttr ".cbx" -type "double3" 0.43075027935724797 0.43075020538056064 -2.9415980676011477 ;
createNode polyTweak -n "polyTweak9";
	rename -uid "EA6ED2E2-4DED-5DE8-FF2F-4B931C092672";
	setAttr ".uopa" yes;
	setAttr -s 18 ".tk";
	setAttr ".tk[0]" -type "float3" -0.0061273319 0.0010831672 0.0061273184 ;
	setAttr ".tk[1]" -type "float3" 8.8395269e-10 0.0010831672 0.0086653493 ;
	setAttr ".tk[2]" -type "float3" 0.0061273184 0.0010831672 0.0061273184 ;
	setAttr ".tk[3]" -type "float3" 0.0086653493 0.0010831672 2.5824748e-10 ;
	setAttr ".tk[4]" -type "float3" 0.0061273184 0.0010831672 -0.0061273184 ;
	setAttr ".tk[5]" -type "float3" 8.8395269e-10 0.0010831672 -0.0086653493 ;
	setAttr ".tk[6]" -type "float3" -0.0061273337 0.0010831672 -0.0061273184 ;
	setAttr ".tk[7]" -type "float3" -0.0086653493 0.0010831672 2.5824748e-10 ;
	setAttr ".tk[34]" -type "float3" -0.0061273337 -0.0010831672 0.0061273184 ;
	setAttr ".tk[35]" -type "float3" -0.0086653493 -0.0010831672 2.5824748e-10 ;
	setAttr ".tk[36]" -type "float3" -0.0061273337 -0.0010831672 -0.0061273184 ;
	setAttr ".tk[37]" -type "float3" 8.8395269e-10 -0.0010831672 -0.0086653549 ;
	setAttr ".tk[38]" -type "float3" 0.0061273184 -0.0010831672 -0.0061273184 ;
	setAttr ".tk[39]" -type "float3" 0.0086653493 -0.0010831672 2.5824748e-10 ;
	setAttr ".tk[40]" -type "float3" 0.0061273184 -0.0010831672 0.0061273184 ;
	setAttr ".tk[41]" -type "float3" 8.8395269e-10 -0.0010831672 0.0086653549 ;
createNode polyExtrudeFace -n "polyExtrudeFace4";
	rename -uid "A1CAE74F-4C15-A73B-F670-92BA83F8E18B";
	setAttr ".ics" -type "componentList" 1 "f[8:15]";
	setAttr ".ix" -type "matrix" 0.62056144267389024 0 0 0 0 -1.6572899233542062e-15 3.7318851406314404 0
		 0 -0.62056144267389024 -2.7558464074046285e-16 0 0 0 0 1;
	setAttr ".ws" yes;
	setAttr ".pvt" -type "float3" 3.6988343e-08 -3.6988343e-08 -3.660543 ;
	setAttr ".rs" 59026;
	setAttr ".ls" -type "double3" 0.29999999619725576 0.29999999619725576 0.29999999619725576 ;
	setAttr ".c[0]"  0 1 1;
	setAttr ".cbn" -type "double3" -0.43075016839221492 -0.43075024236890203 -3.7318851406314404 ;
	setAttr ".cbx" -type "double3" 0.43075024236890364 0.43075016839221653 -3.5892009285038728 ;
createNode polySoftEdge -n "polySoftEdge5";
	rename -uid "5134BDA3-4BC6-9408-9571-45870282E394";
	setAttr ".uopa" yes;
	setAttr ".ics" -type "componentList" 7 "e[146:148]" "e[150:151]" "e[153:154]" "e[156:157]" "e[159:160]" "e[162:163]" "e[165:167]";
	setAttr ".ix" -type "matrix" 0.62056144267389024 0 0 0 0 -1.6572899233542062e-15 3.7318851406314404 0
		 0 -0.62056144267389024 -2.7558464074046285e-16 0 0 0 0 1;
	setAttr ".a" 0;
createNode polyTweak -n "polyTweak10";
	rename -uid "7BA554ED-45EA-54F9-4823-DF91692DAD08";
	setAttr ".uopa" yes;
	setAttr -s 9 ".tk[73:81]" -type "float3"  1.7136335e-07 0.10615467 -5.9604645e-08
		 2.6645353e-15 0.10615461 -1.4156103e-07 0 0.13457221 -7.9936058e-15 -1.5646219e-07
		 0.10615472 -5.9604645e-08 -2.0861626e-07 0.10615478 -1.5099033e-14 -1.5646219e-07
		 0.10615472 9.6857548e-08 -2.6645353e-15 0.10615461 1.4156103e-07 1.5646219e-07 0.10615461
		 9.6857548e-08 2.0861626e-07 0.10615467 -1.9539925e-14;
createNode polySoftEdge -n "polySoftEdge6";
	rename -uid "3E6AA7FA-45D3-5310-E211-AB828E5D41E4";
	setAttr ".uopa" yes;
	setAttr ".ics" -type "componentList" 1 "e[0:7]";
	setAttr ".ix" -type "matrix" 0.62056144267389024 0 0 0 0 -1.6572899233542062e-15 3.7318851406314404 0
		 0 -0.62056144267389024 -2.7558464074046285e-16 0 0 0 0 1;
	setAttr ".a" 0;
createNode polyTweak -n "polyTweak11";
	rename -uid "FA0BEEAE-4958-9A3A-53F4-7EB2C8306940";
	setAttr ".uopa" yes;
	setAttr -s 81 ".tk";
	setAttr ".tk[0]" -type "float3" -0.24077238 -0.11282223 0.2407725 ;
	setAttr ".tk[1]" -type "float3" 2.0115356e-08 -0.11282223 0.34050381 ;
	setAttr ".tk[2]" -type "float3" 0.24077244 -0.11282223 0.2407725 ;
	setAttr ".tk[3]" -type "float3" 0.34050381 -0.11282223 2.4767255e-08 ;
	setAttr ".tk[4]" -type "float3" 0.24077244 -0.11282223 -0.24077244 ;
	setAttr ".tk[5]" -type "float3" 2.0115356e-08 -0.11282223 -0.34050381 ;
	setAttr ".tk[6]" -type "float3" -0.24077244 -0.11282223 -0.24077244 ;
	setAttr ".tk[7]" -type "float3" -0.34050381 -0.11282223 2.4767255e-08 ;
	setAttr ".tk[9]" -type "float3" -0.060304508 0.23594156 0.0603045 ;
	setAttr ".tk[10]" -type "float3" -0.085283421 0.23594156 -2.088858e-09 ;
	setAttr ".tk[11]" -type "float3" -0.060304508 0.23594156 -0.060304496 ;
	setAttr ".tk[12]" -type "float3" 5.083288e-09 0.23594156 -0.085283406 ;
	setAttr ".tk[13]" -type "float3" 0.060304508 0.23594156 -0.060304496 ;
	setAttr ".tk[14]" -type "float3" 0.085283421 0.23594156 -2.088858e-09 ;
	setAttr ".tk[15]" -type "float3" 0.060304508 0.23594156 0.0603045 ;
	setAttr ".tk[16]" -type "float3" 5.083288e-09 0.23594156 0.085283406 ;
	setAttr ".tk[17]" -type "float3" -0.061761085 -0.023395808 0.061761077 ;
	setAttr ".tk[18]" -type "float3" -0.087343387 -0.023395808 -2.0274693e-09 ;
	setAttr ".tk[19]" -type "float3" -0.061761085 -0.023395808 -0.061761096 ;
	setAttr ".tk[20]" -type "float3" 5.2060725e-09 -0.023395808 -0.087343387 ;
	setAttr ".tk[21]" -type "float3" 0.061761085 -0.023395808 -0.061761096 ;
	setAttr ".tk[22]" -type "float3" 0.087343387 -0.023395808 -2.0274693e-09 ;
	setAttr ".tk[23]" -type "float3" 0.061761085 -0.023395808 0.061761077 ;
	setAttr ".tk[24]" -type "float3" 5.2060725e-09 -0.023395808 0.087343387 ;
	setAttr ".tk[25]" -type "float3" 1.4901161e-08 -2.9802322e-08 -4.4703484e-08 ;
	setAttr ".tk[26]" -type "float3" -2.9802322e-08 -2.9802322e-08 8.8817842e-16 ;
	setAttr ".tk[27]" -type "float3" 1.4901161e-08 -2.9802322e-08 1.4901161e-08 ;
	setAttr ".tk[28]" -type "float3" 1.7763568e-15 -2.9802322e-08 -2.9802322e-08 ;
	setAttr ".tk[29]" -type "float3" -1.4901161e-08 -2.9802322e-08 -2.2351742e-08 ;
	setAttr ".tk[30]" -type "float3" 2.9802322e-08 -2.9802322e-08 8.8817842e-16 ;
	setAttr ".tk[31]" -type "float3" -1.4901161e-08 -2.9802322e-08 -4.4703484e-08 ;
	setAttr ".tk[32]" -type "float3" 1.7763568e-15 -2.9802322e-08 2.9802322e-08 ;
	setAttr ".tk[33]" -type "float3" -0.061429445 0.13791135 0.061429415 ;
	setAttr ".tk[34]" -type "float3" -0.086874537 0.13791135 6.3189805e-09 ;
	setAttr ".tk[35]" -type "float3" -0.061429445 0.13791135 -0.061429445 ;
	setAttr ".tk[36]" -type "float3" 5.132117e-09 0.13791135 -0.086874537 ;
	setAttr ".tk[37]" -type "float3" 0.061429448 0.13791135 -0.061429445 ;
	setAttr ".tk[38]" -type "float3" 0.086874537 0.13791135 6.3189805e-09 ;
	setAttr ".tk[39]" -type "float3" 0.061429448 0.13791135 0.061429415 ;
	setAttr ".tk[40]" -type "float3" 5.132117e-09 0.13791135 0.086874492 ;
	setAttr ".tk[41]" -type "float3" -4.4703484e-08 1.4901161e-08 0 ;
	setAttr ".tk[42]" -type "float3" 1.3411045e-07 1.4901161e-08 8.8817842e-16 ;
	setAttr ".tk[43]" -type "float3" -0.069095686 0.23846187 0.069095671 ;
	setAttr ".tk[44]" -type "float3" -0.097716078 0.23846187 -2.0986342e-09 ;
	setAttr ".tk[45]" -type "float3" 0 1.4901161e-08 0 ;
	setAttr ".tk[46]" -type "float3" -0.069095686 0.23846187 -0.069095694 ;
	setAttr ".tk[47]" -type "float3" 2.6645353e-15 1.4901161e-08 1.3411045e-07 ;
	setAttr ".tk[48]" -type "float3" 5.5301146e-09 0.23846187 -0.097716078 ;
	setAttr ".tk[49]" -type "float3" 0 1.4901161e-08 -4.4703484e-08 ;
	setAttr ".tk[50]" -type "float3" 0.069095694 0.23846187 -0.069095694 ;
	setAttr ".tk[51]" -type "float3" -1.3411045e-07 1.4901161e-08 -2.6645353e-15 ;
	setAttr ".tk[52]" -type "float3" 0.097716078 0.23846187 -2.3318156e-09 ;
	setAttr ".tk[53]" -type "float3" 1.4901161e-08 1.4901161e-08 0 ;
	setAttr ".tk[54]" -type "float3" 0.069095686 0.23846187 0.069095671 ;
	setAttr ".tk[55]" -type "float3" 0 1.4901161e-08 -1.3411045e-07 ;
	setAttr ".tk[56]" -type "float3" 5.2969304e-09 0.23846187 0.097716078 ;
	setAttr ".tk[57]" -type "float3" -0.31427157 -0.11949044 0.31427163 ;
	setAttr ".tk[58]" -type "float3" 1.8932091e-08 -0.11949044 0.44444734 ;
	setAttr ".tk[59]" -type "float3" 4.8302429e-09 0.13961276 0.11339372 ;
	setAttr ".tk[60]" -type "float3" -0.08018139 0.13961276 0.080181457 ;
	setAttr ".tk[61]" -type "float3" 0.31427163 -0.11949044 0.31427163 ;
	setAttr ".tk[62]" -type "float3" 0.080181457 0.13961276 0.080181457 ;
	setAttr ".tk[63]" -type "float3" 0.44444734 -0.11949044 2.48217e-08 ;
	setAttr ".tk[64]" -type "float3" 0.11339372 0.13961276 6.3328796e-09 ;
	setAttr ".tk[65]" -type "float3" 0.31427163 -0.11949044 -0.3142716 ;
	setAttr ".tk[66]" -type "float3" 0.080181457 0.13961276 -0.08018139 ;
	setAttr ".tk[67]" -type "float3" 2.1876907e-08 -0.11949044 -0.44444734 ;
	setAttr ".tk[68]" -type "float3" 5.5815526e-09 0.13961276 -0.11339372 ;
	setAttr ".tk[69]" -type "float3" -0.3142716 -0.11949044 -0.3142716 ;
	setAttr ".tk[70]" -type "float3" -0.08018139 0.13961276 -0.08018139 ;
	setAttr ".tk[71]" -type "float3" -0.44444734 -0.11949044 3.6600898e-08 ;
	setAttr ".tk[72]" -type "float3" -0.11339372 0.13961276 7.4598869e-09 ;
	setAttr ".tk[73]" -type "float3" -0.13742451 0.1475727 0.13742444 ;
	setAttr ".tk[74]" -type "float3" 1.4893168e-08 0.1475727 0.19434729 ;
	setAttr ".tk[75]" -type "float3" 7.4598869e-09 0.1475727 7.4598807e-09 ;
	setAttr ".tk[76]" -type "float3" 0.13742441 0.1475727 0.13742444 ;
	setAttr ".tk[77]" -type "float3" 0.19434734 0.1475727 2.2794048e-08 ;
	setAttr ".tk[78]" -type "float3" 0.13742441 0.1475727 -0.13742448 ;
	setAttr ".tk[79]" -type "float3" 2.6579516e-11 0.1475727 -0.19434732 ;
	setAttr ".tk[80]" -type "float3" -0.1374245 0.1475727 -0.13742448 ;
	setAttr ".tk[81]" -type "float3" -0.19434726 0.1475727 3.766062e-08 ;
createNode transformGeometry -n "transformGeometry2";
	rename -uid "69C9A4A7-4E31-FF61-3757-ECA59F3EA5B0";
	setAttr ".txf" -type "matrix" 0.62056144267389024 0 0 0 0 -1.6572899233542062e-15 3.7318851406314404 0
		 0 -0.62056144267389024 -2.7558464074046285e-16 0 0 0 0.85241694920361044 1;
select -ne :time1;
	setAttr ".o" 1;
	setAttr ".unw" 1;
select -ne :hardwareRenderingGlobals;
	setAttr ".otfna" -type "stringArray" 22 "NURBS Curves" "NURBS Surfaces" "Polygons" "Subdiv Surface" "Particles" "Particle Instance" "Fluids" "Strokes" "Image Planes" "UI" "Lights" "Cameras" "Locators" "Joints" "IK Handles" "Deformers" "Motion Trails" "Components" "Hair Systems" "Follicles" "Misc. UI" "Ornaments"  ;
	setAttr ".otfva" -type "Int32Array" 22 0 1 1 1 1 1
		 1 1 1 0 0 0 0 0 0 0 0 0
		 0 0 0 0 ;
	setAttr ".fprt" yes;
select -ne :renderPartition;
	setAttr -s 2 ".st";
select -ne :renderGlobalsList1;
select -ne :defaultShaderList1;
	setAttr -s 5 ".s";
select -ne :postProcessList1;
	setAttr -s 2 ".p";
select -ne :defaultRenderingList1;
select -ne :initialShadingGroup;
	setAttr -s 2 ".dsm";
	setAttr ".ro" yes;
select -ne :initialParticleSE;
	setAttr ".ro" yes;
select -ne :defaultRenderGlobals;
	addAttr -ci true -h true -sn "dss" -ln "defaultSurfaceShader" -dt "string";
	setAttr ".dss" -type "string" "lambert1";
select -ne :defaultResolution;
	setAttr ".pa" 1;
select -ne :hardwareRenderGlobals;
	setAttr ".ctrs" 256;
	setAttr ".btrs" 512;
select -ne :ikSystem;
	setAttr -s 4 ".sol";
connectAttr "polySoftEdge2.out" "Hardlight_BulletShape.i";
connectAttr "transformGeometry2.og" "Impact_MissileShape.i";
relationship "link" ":lightLinker1" ":initialShadingGroup.message" ":defaultLightSet.message";
relationship "link" ":lightLinker1" ":initialParticleSE.message" ":defaultLightSet.message";
relationship "shadowLink" ":lightLinker1" ":initialShadingGroup.message" ":defaultLightSet.message";
relationship "shadowLink" ":lightLinker1" ":initialParticleSE.message" ":defaultLightSet.message";
connectAttr "layerManager.dli[0]" "defaultLayer.id";
connectAttr "renderLayerManager.rlmi[0]" "defaultRenderLayer.rlid";
connectAttr "polyCube1.out" "polyExtrudeFace1.ip";
connectAttr "Hardlight_BulletShape.wm" "polyExtrudeFace1.mp";
connectAttr "polyTweak1.out" "polyMergeVert1.ip";
connectAttr "Hardlight_BulletShape.wm" "polyMergeVert1.mp";
connectAttr "polyExtrudeFace1.out" "polyTweak1.ip";
connectAttr "polyTweak2.out" "polyMergeVert2.ip";
connectAttr "Hardlight_BulletShape.wm" "polyMergeVert2.mp";
connectAttr "polyMergeVert1.out" "polyTweak2.ip";
connectAttr "polyTweak3.out" "polySoftEdge1.ip";
connectAttr "Hardlight_BulletShape.wm" "polySoftEdge1.mp";
connectAttr "polyMergeVert2.out" "polyTweak3.ip";
connectAttr "polySoftEdge1.out" "polyTweak4.ip";
connectAttr "polyTweak4.out" "transformGeometry1.ig";
connectAttr "transformGeometry1.og" "polySplit1.ip";
connectAttr "polyTweak5.out" "polyCircularize1.ip";
connectAttr "Hardlight_BulletShape.wm" "polyCircularize1.mp";
connectAttr "polySplit1.out" "polyTweak5.ip";
connectAttr "polyCircularize1.out" "polySoftEdge2.ip";
connectAttr "Hardlight_BulletShape.wm" "polySoftEdge2.mp";
connectAttr "polyCylinder1.out" "polySplitRing1.ip";
connectAttr "Impact_MissileShape.wm" "polySplitRing1.mp";
connectAttr "polySplitRing1.out" "polySplitRing2.ip";
connectAttr "Impact_MissileShape.wm" "polySplitRing2.mp";
connectAttr "polySplitRing2.out" "polySplitRing3.ip";
connectAttr "Impact_MissileShape.wm" "polySplitRing3.mp";
connectAttr "polySplitRing3.out" "polySplitRing4.ip";
connectAttr "Impact_MissileShape.wm" "polySplitRing4.mp";
connectAttr "polyTweak6.out" "polyMergeVert3.ip";
connectAttr "Impact_MissileShape.wm" "polyMergeVert3.mp";
connectAttr "polySplitRing4.out" "polyTweak6.ip";
connectAttr "polyTweak7.out" "polyExtrudeFace2.ip";
connectAttr "Impact_MissileShape.wm" "polyExtrudeFace2.mp";
connectAttr "polyMergeVert3.out" "polyTweak7.ip";
connectAttr "polyTweak8.out" "polySoftEdge3.ip";
connectAttr "Impact_MissileShape.wm" "polySoftEdge3.mp";
connectAttr "polyExtrudeFace2.out" "polyTweak8.ip";
connectAttr "polySoftEdge3.out" "polySoftEdge4.ip";
connectAttr "Impact_MissileShape.wm" "polySoftEdge4.mp";
connectAttr "polyTweak9.out" "polyExtrudeFace3.ip";
connectAttr "Impact_MissileShape.wm" "polyExtrudeFace3.mp";
connectAttr "polySoftEdge4.out" "polyTweak9.ip";
connectAttr "polyExtrudeFace3.out" "polyExtrudeFace4.ip";
connectAttr "Impact_MissileShape.wm" "polyExtrudeFace4.mp";
connectAttr "polyTweak10.out" "polySoftEdge5.ip";
connectAttr "Impact_MissileShape.wm" "polySoftEdge5.mp";
connectAttr "polyExtrudeFace4.out" "polyTweak10.ip";
connectAttr "polySoftEdge5.out" "polySoftEdge6.ip";
connectAttr "Impact_MissileShape.wm" "polySoftEdge6.mp";
connectAttr "polySoftEdge6.out" "polyTweak11.ip";
connectAttr "polyTweak11.out" "transformGeometry2.ig";
connectAttr "defaultRenderLayer.msg" ":defaultRenderingList1.r" -na;
connectAttr "Hardlight_BulletShape.iog" ":initialShadingGroup.dsm" -na;
connectAttr "Impact_MissileShape.iog" ":initialShadingGroup.dsm" -na;
// End of Gun Ammo Models.ma
