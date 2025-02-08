shader_type spatial;

uniform float wave_speed = 0.2;
uniform float wave_height = 5.0f;
uniform vec4 water_color:source_color;
uniform float stylize_amount:hint_range(0.01,7,0.01) = 1.0;

uniform sampler2D water_layer1;
uniform sampler2D water_layer2;

float animate_water(vec2 uv, float time){
	vec3 water_text1 = texture(water_layer1, uv + time * 0.08 * wave_speed).rgb;
	vec3 water_text2 = texture(water_layer2, uv - time * 0.02 * wave_speed).rgb;
	
	return (water_text1.r + water_text2.r) - 1.0f;
}

float stylize_water(float val)
{
	val = val * stylize_amount;
	if(val < -0.0 && val >= -0.2) return -0.01;
	if(val < -0.2 && val >= -0.35) return -0.07;
	if(val < -0.35 && val >= -1.0) return -0.12;
	if(val > 0.0 && val <= 0.15) return 0.2;
	if(val > 0.15 && val <= 0.35) return 0.3;
	if(val > 0.35 && val <= 0.5) return 0.5;
	if(val > 0.5 && val <= 1.0) return 0.8;
	return 1.0;
}

void fragment(){
	
	float water_anim = animate_water(UV, TIME);
	float stylized = stylize_water(water_anim);
	
	ALBEDO = vec3(stylized) + water_color.rgb;
}
