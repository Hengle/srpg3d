﻿using UnityEngine;
using System;
using System.Collections;

// will probably need to rewrite to convert 2d axis data to angle + % magnitude

class InputWrapper {
	string axis1, axis2;
	bool snap;
	float lastTime;
	float initspd, addspd, maxspd, minspd, inputdelay;
	float d1, d2, dt, curspd, timeheld;
	float highthresh = 0.80f, midthresh = 0.3f, lowthresh = 0.2f, deadthresh = 0.1f;
	bool f1, f2;

	public InputWrapper(string _axis1, string _axis2 = "", bool _snap = true) {
		axis1 = _axis1;
		axis2 = _axis2;
		snap = _snap;
		setSpeed(6f, 2f); // default initial speed | acceleration
		release();
	}

    public void setSnap(bool _snap) {
        snap = _snap;
    }

	public void setSpeed(float _initspd, float _addspd, float _maxspd = 16, float _minspd = -1, float _inputdelay = 0.02f) {
		curspd = initspd = _initspd;
		addspd = _addspd;
		maxspd = _maxspd;
		minspd = _minspd;
        if (minspd < 0-Mathf.Epsilon) minspd = initspd;
		inputdelay = _inputdelay;
	}

	private void release() {
        // called upon button release
		dt=d1=d2=timeheld=0;
		f1=f2=true;
	}

    private void press() {
        // called upon button press
        return;
    }

	private void addDir(float dir, ref float dD, ref bool fP) { // reads snap + thresholds from global
		float mag = Mathf.Abs(dir);
		float sign = Mathf.Sign(dir);
		if (mag > deadthresh) {
            if (fP) {
                press(); // triggers first button pression actions
                // curspd = initspd; // curspd should decay towards initspd
                if (snap) { // special case, if snapping we want first input to trigger regardless of input magnitude
                    dD = sign; // gets sign of direction, initial burst of speed
                }
            }
			else if (mag > highthresh) { // increase speed
				dD += sign*curspd*dt; // d = v * t
				curspd = curspd + addspd * dt; // v_o = v_i + a * t;
				curspd = Mathf.Min(curspd, maxspd);
			}
			else if (mag > midthresh) { // constant speed
				dD += sign*curspd*dt;
			}
			else { // decrease speed
				dD += sign*curspd*dt; // d = v * t
				curspd = curspd - addspd * dt; // v_o = v_i + a * t;
				curspd = Mathf.Max(curspd, minspd);
			}
		}
	}

	public Vec Update() {
		Vec v = new Vec(Input.GetAxisRaw(axis1), axis2.Length>0 ? Input.GetAxisRaw(axis2) : 0f), ret;
        // v.x = axis 1, v.y = axis 2 or 0 if unset

        // get dT from last time
        float curTime = Time.time;
        if (curTime > lastTime) {
            dt = curTime-lastTime;
        }
        lastTime = curTime;

        float largerAxis = Mathf.Max(Mathf.Abs(v.x), Mathf.Abs(v.y));
        if (largerAxis > lowthresh) {
            // sets delay before input registration
            // dt = time since last frame during hold, or 0 if pressed
            timeheld += dt;

            if (timeheld > inputdelay) {
                // we put axis data + speed function into d1/d2, f1/f2 signifies if it is the first instance of button push
                addDir(v.x, ref d1, ref f1);
                addDir(v.y, ref d2, ref f2);
                f1 = f2 = false;
            }
            //Debug.Log(curspd);
        }
        else if (timeheld > inputdelay) {
            release();
        }
        else {
            float diff = initspd-curspd;
            float relativeSign = Mathf.Sign(diff);
            if (Mathf.Abs(diff) > addspd*4*dt) curspd = Mathf.Max(curspd + relativeSign * addspd*4*dt, initspd); // decay our speed if it's faster, otherwise reset at rate of 4x addspd per second
            else curspd = initspd; // close enough to we stop decaying
            // Debug.Log(curspd);
		}
		
		float x=0f, y=0f;
		if (Mathf.Abs(d1) > 1-float.Epsilon) {
			x = Mathf.Sign(d1); // normalize while keeping the sign;
			d1 = 0f;
		}
		else if (!snap) {
			x = d1;
			d1 = 0f;
		}
		if (Mathf.Abs(d2) > 1-float.Epsilon) {
			y = Mathf.Sign(d2); // normalize while keeping the sign;
			d2 = 0f;
		}
		else if (!snap) {
			y = d2;
			d2 = 0f;
		}

        ret = new Vec(x, y);
        if (!snap) ret = ret * Mathf.Abs(largerAxis);

		return ret;
	}
}

public class InputController: MonoBehaviour {
	public static event EventHandler<InfoEventArgs<Vec>> moveEvent;
	public static event EventHandler<InfoEventArgs<int>> fireEvent;

	InputWrapper dir = new InputWrapper("Horizontal", "Vertical", false);

	string[] _buttons = new string[] { "Fire1", "Fire2", "Fire3" };

    void Awake() {
        // dir.setSnap(true);
    }

	void Update() {
		Vec input = dir.Update();
		float x = input.x, y = input.y;
		if (x != 0f || y != 0f) {
			// if (true) Debug.Log(string.Format("X:{0}, Y:{1}", x, y)); // debugging
			if (moveEvent != null)
				moveEvent(this, new InfoEventArgs<Vec>(new Vec(x, y)));
		}

		for (int i = 0; i < 3; ++i) {
			if (Input.GetButtonUp(_buttons[i])) {
				if (fireEvent != null)
					fireEvent(this, new InfoEventArgs<int>(i));
			}
		}
	}
}
