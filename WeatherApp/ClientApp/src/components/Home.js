import React, { Component } from 'react';
import { Cities } from './Cities';

export class Home extends Component {
  static displayName = Home.name;

  render() {
    return (
      <div>
        <p>Welcome Weather app page which represents following cities:</p>
        <Cities />
      </div>
    );
  }
}
